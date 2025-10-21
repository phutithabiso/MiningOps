using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models;

namespace MiningOps.Controllers
{
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(AppDbContext context, ILogger<InventoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Index (unchanged)
        public async Task<IActionResult> Index(
            string? searchString,
            string? warehouseFilter,
            string? categoryFilter,
            string? stockStatusFilter,
            string? sortOrder,
            int? pageNumber,
            int pageSize = 10)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewData["QuantitySortParm"] = sortOrder == "quantity" ? "quantity_desc" : "quantity";
                ViewData["CostSortParm"] = sortOrder == "cost" ? "cost_desc" : "cost";
                ViewData["WarehouseSortParm"] = sortOrder == "warehouse" ? "warehouse_desc" : "warehouse";
                ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";

                var inventoryQuery = _context.InventoryDb
                    .Include(i => i.Warehouse)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    inventoryQuery = inventoryQuery.Where(i =>
                        i.ItemName!.Contains(searchString) ||
                        (i.Description != null && i.Description.Contains(searchString)) ||
                        (i.Unit != null && i.Unit.Contains(searchString)));
                }

                if (!string.IsNullOrEmpty(warehouseFilter) && int.TryParse(warehouseFilter, out int warehouseId))
                {
                    inventoryQuery = inventoryQuery.Where(i => i.WarehouseId == warehouseId);
                }

                if (!string.IsNullOrEmpty(categoryFilter))
                {
                    inventoryQuery = inventoryQuery.Where(i => i.ItemName == categoryFilter);
                }

                if (!string.IsNullOrEmpty(stockStatusFilter))
                {
                    inventoryQuery = stockStatusFilter switch
                    {
                        "outofstock" => inventoryQuery.Where(i => i.Quantity <= 0),
                        "lowstock" => inventoryQuery.Where(i => i.Quantity > 0 && i.Quantity <= i.ReorderLevel),
                        "instock" => inventoryQuery.Where(i => i.Quantity > i.ReorderLevel),
                        _ => inventoryQuery
                    };
                }

                inventoryQuery = sortOrder switch
                {
                    "name_desc" => inventoryQuery.OrderByDescending(i => i.ItemName),
                    "quantity" => inventoryQuery.OrderBy(i => i.Quantity),
                    "quantity_desc" => inventoryQuery.OrderByDescending(i => i.Quantity),
                    "cost" => inventoryQuery.OrderBy(i => i.UnitCost),
                    "cost_desc" => inventoryQuery.OrderByDescending(i => i.UnitCost),
                    "warehouse" => inventoryQuery.OrderBy(i => i.Warehouse.Name),
                    "warehouse_desc" => inventoryQuery.OrderByDescending(i => i.Warehouse.Name),
                    "date" => inventoryQuery.OrderBy(i => i.LastUpdated),
                    "date_desc" => inventoryQuery.OrderByDescending(i => i.LastUpdated),
                    _ => inventoryQuery.OrderBy(i => i.ItemName)
                };

                var warehouses = await _context.WarehousesDb
                    .Select(w => new { w.WarehouseId, w.Name })
                    .OrderBy(w => w.Name)
                    .ToListAsync();

                var categories = await _context.InventoryDb
                    .Where(i => i.ItemName != null)
                    .Select(i => i.ItemName)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();

                ViewBag.Warehouses = new SelectList(warehouses, "WarehouseId", "Name");
                ViewBag.Categories = categories;

                var totalItems = await inventoryQuery.CountAsync();
                var totalValue = await inventoryQuery.SumAsync(i => (decimal?)(i.Quantity * i.UnitCost)) ?? 0;
                var outOfStockCount = await inventoryQuery.CountAsync(i => i.Quantity <= 0);
                var lowStockCount = await inventoryQuery.CountAsync(i => i.Quantity > 0 && i.Quantity <= i.ReorderLevel);

                ViewBag.TotalItems = totalItems;
                ViewBag.TotalValue = totalValue;
                ViewBag.OutOfStockCount = outOfStockCount;
                ViewBag.LowStockCount = lowStockCount;
                ViewBag.InStockCount = totalItems - outOfStockCount - lowStockCount;

                var paginatedItems = await PaginatedList<InventoryItem>.CreateAsync(
                    inventoryQuery.AsNoTracking(),
                    pageNumber ?? 1,
                    pageSize);

                return View(paginatedItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading inventory index page");
                TempData["ErrorMessage"] = "An error occurred while loading inventory data.";
                return View(new List<InventoryItem>());
            }
        }

        // ✅ CREATE
        public IActionResult Create()
        {
            ViewBag.Warehouses = _context.WarehousesDb
                .Select(w => new SelectListItem
                {
                    Value = w.WarehouseId.ToString(),
                    Text = w.Name
                })
                .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvetoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var inventory = new InventoryItem
                    {
                        ItemName = model.ItemName,
                        Quantity = model.Quantity,
                        UnitCost = model.UnitCost,
                        Description = model.Description,
                        WarehouseId = model.WarehouseId,
                        LastUpdated = DateTime.UtcNow
                    };

                    _context.InventoryDb.Add(inventory);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Inventory item created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating inventory item");
                    TempData["ErrorMessage"] = "Something went wrong while adding inventory item.";
                }
            }

            // Re-populate dropdown after validation failure
            ViewBag.Warehouses = _context.WarehousesDb
                .Select(w => new SelectListItem
                {
                    Value = w.WarehouseId.ToString(),
                    Text = w.Name
                })
                .ToList();

            return View(model);
        }

        // ✅ EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.InventoryDb.FindAsync(id);
            if (inventory == null) return NotFound();

            var model = new InvetoryViewModel
            {
                ItemName = inventory.ItemName,
                Quantity = inventory.Quantity,
                UnitCost = inventory.UnitCost,
                Description = inventory.Description,
                WarehouseId = inventory.WarehouseId
            };

            ViewBag.Warehouses = _context.WarehousesDb
                .Select(w => new SelectListItem
                {
                    Value = w.WarehouseId.ToString(),
                    Text = w.Name
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InvetoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var inventory = await _context.InventoryDb.FindAsync(id);
                    if (inventory == null) return NotFound();

                    inventory.ItemName = model.ItemName;
                    inventory.Quantity = model.Quantity;
                    inventory.UnitCost = model.UnitCost;
                    inventory.Description = model.Description;
                    inventory.WarehouseId = model.WarehouseId;
                    inventory.LastUpdated = DateTime.UtcNow;

                    _context.Update(inventory);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Inventory item updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating inventory");
                    TempData["ErrorMessage"] = "Unexpected error while updating inventory.";
                }
            }

            ViewBag.Warehouses = _context.WarehousesDb
                .Select(w => new SelectListItem
                {
                    Value = w.WarehouseId.ToString(),
                    Text = w.Name
                })
                .ToList();

            return View(model);
        }

        // ✅ DELETE
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.InventoryDb
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.InventoryId == id);

            if (inventory == null) return NotFound();

            return View(inventory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var inventory = await _context.InventoryDb.FindAsync(id);
                if (inventory == null) return NotFound();

                _context.InventoryDb.Remove(inventory);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Inventory item deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting inventory {InventoryId}", id);
                TempData["ErrorMessage"] = "Failed to delete inventory. It may be linked to existing orders.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
