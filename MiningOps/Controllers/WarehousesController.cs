using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models;

namespace MiningOps.Controllers
{
    public class WarehousesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WarehousesController> _logger;

        public WarehousesController(AppDbContext context, ILogger<WarehousesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Warehouses
        public async Task<IActionResult> Index()
        {
            var warehouses = await _context.WarehousesDb.ToListAsync();
            return View(warehouses);
        }

        // GET: Warehouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var warehouse = await _context.WarehousesDb
                .Include(w => w.InventoryItems)
                .FirstOrDefaultAsync(m => m.WarehouseId == id);

            if (warehouse == null) return NotFound();

            return View(warehouse);
        }

        // GET: Warehouses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Warehouses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WarehouseViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var warehouse = new Warehouse
                    {
                       
                        Name = model.Name,
                        Location = model.Location,
                        Notes = model.Notes,
                        
                    };

                    _context.WarehousesDb.Add(warehouse);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Warehouse created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating warehouse");
                    TempData["ErrorMessage"] = "Something went wrong while creating warehouse.";
                }
            }
            return View(model);
        }

        // GET: Warehouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var warehouse = await _context.WarehousesDb.FindAsync(id);
            if (warehouse == null) return NotFound();

            // Map to ViewModel
            var model = new WarehouseViewModel
            {
                
                Name = warehouse.Name,
                Location = warehouse.Location,
                Notes = warehouse.Notes,
                CreatedAt = warehouse.CreatedAt
            };

            return View(model);
        }

        // POST: Warehouses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WarehouseViewModel model)
        {
        

            if (ModelState.IsValid)
            {
                try
                {
                    var warehouse = await _context.WarehousesDb.FindAsync(id);
                    if (warehouse == null) return NotFound();

                    // Update entity
                    warehouse.Name = model.Name;
                    warehouse.Location = model.Location;
                   warehouse.Notes = model.Notes;

                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Warehouse updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error updating warehouse {WarehouseId}", id);
                    if (!_context.WarehousesDb.Any(e => e.WarehouseId == id))
                        return NotFound();

                    TempData["ErrorMessage"] = "Warehouse update failed. Please try again.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating warehouse");
                    TempData["ErrorMessage"] = "Unexpected error while updating warehouse.";
                }
            }
            return View(model);
        }

        // GET: Warehouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var warehouse = await _context.WarehousesDb.FirstOrDefaultAsync(m => m.WarehouseId == id);
            if (warehouse == null) return NotFound();

            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var warehouse = await _context.WarehousesDb.FindAsync(id);
                if (warehouse == null) return NotFound();

                _context.WarehousesDb.Remove(warehouse);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Warehouse deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting warehouse {WarehouseId}", id);
                TempData["ErrorMessage"] = "Failed to delete warehouse. It may be linked to inventory.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
