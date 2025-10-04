using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models;

namespace MiningOps.Controllers
{
    [Authorize] // Require login for all actions
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(AppDbContext context, ILogger<InventoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ All roles can view inventory
        [Authorize(Roles = "Admin,Supervisor,Supplier")]
        public async Task<IActionResult> Index()
        {
            var inventoryItems = await _context.InventoryDb
                .Include(i => i.Warehouse)
                .ToListAsync();

            return View(inventoryItems);
        }

        // ✅ All roles can view details
        [Authorize(Roles = "Admin,Supervisor,Supplier")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.InventoryDb
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.InventoryId == id);

            if (item == null) return NotFound();

            return View(item);
        }

        // ✅ Only Admin and Supervisor can add inventory
        [Authorize(Roles = "Admin,Supervisor")]
        public IActionResult Create()
        {
            ViewData["Warehouses"] = _context.WarehousesDb.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor")]
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

            ViewData["Warehouses"] = _context.WarehousesDb.ToList();
            return View(model);
        }

        // ✅ Only Admin and Supervisor can edit
        [Authorize(Roles = "Admin,Supervisor")]
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
                WarehouseId = inventory.WarehouseId,
                LastUpdated = DateTime.UtcNow
            };

            ViewData["Warehouses"] = _context.WarehousesDb.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor")]
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

            ViewData["Warehouses"] = _context.WarehousesDb.ToList();
            return View(model);
        }

        // ✅ Only Admin can delete
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
