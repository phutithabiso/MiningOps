using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MiningOps.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly AppDbContext _context;

        public OrderItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index()
        {
            var items = await _context.OrderItemsDb
           
                .ToListAsync();

            return View(items);
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.OrderItemsDb
                .Include(o => o.PurchaseOrder)
                    .ThenInclude(po => po.Supplier)
                .FirstOrDefaultAsync(o => o.OrderItemId == id);

            if (item == null) return NotFound();

            return View(item);
        }

        // GET: OrderItems/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: OrderItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderItem model)
        {
            if (ModelState.IsValid)
            {
                if (model.PurchaseOrderId == 0)
                {
                    ModelState.AddModelError("PurchaseOrderId", "Purchase Order is required.");
                    PopulateDropdowns(model);
                    return View(model);
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Order item added successfully!";
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(model);
            return View(model);
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.OrderItemsDb.FindAsync(id);
            if (item == null) return NotFound();

            PopulateDropdowns(item);
            return View(item);
        }

        // POST: OrderItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderItem model)
        {
            if (id != model.OrderItemId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.PurchaseOrderId == 0)
                    {
                        ModelState.AddModelError("PurchaseOrderId", "Purchase Order is required.");
                        PopulateDropdowns(model);
                        return View(model);
                    }

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Order item updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(model.OrderItemId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(model);
            return View(model);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.OrderItemsDb
                .Include(o => o.PurchaseOrder)
                    .ThenInclude(po => po.Supplier)
                .FirstOrDefaultAsync(o => o.OrderItemId == id);

            if (item == null) return NotFound();

            return View(item);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.OrderItemsDb.FindAsync(id);
            if (item != null)  
            {
                _context.OrderItemsDb.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Order item deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItemsDb.Any(e => e.OrderItemId == id);
        }

        /// <summary>
        /// Populates PurchaseOrder dropdown list
        /// </summary>
        private void PopulateDropdowns(OrderItem? model = null)
        {
            // Retrieve role and supplier claims
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == "UserRole")?.Value;
            var supplierClaim = User.Claims.FirstOrDefault(c => c.Type == "SupplierId")?.Value;

            UserRole role = Enum.TryParse(roleClaim, out UserRole parsedRole) ? parsedRole : UserRole.Supervisor;
            int? currentSupplierId = supplierClaim != null ? int.Parse(supplierClaim) : null;

            // Purchase Orders
            var purchaseOrders = _context.PurchaseOrdersDb
                .Include(po => po.Supplier)
                .AsQueryable();

            // If user is a Supplier, show only their own Purchase Orders
            if (role == UserRole.Supplier && currentSupplierId.HasValue)
            {
                purchaseOrders = purchaseOrders.Where(po => po.SupplierId == currentSupplierId.Value);
            }

            // Admin sees all Purchase Orders
            ViewBag.PurchaseOrderId = purchaseOrders
                .Select(po => new SelectListItem
                {
                    Value = po.OrderId.ToString(),
                    Text = $"PO#{po.OrderId} - {po.Supplier.CompanyName}",
                    Selected = model != null && model.PurchaseOrderId == po.OrderId
                })
                .ToList();
        }

    }
}

