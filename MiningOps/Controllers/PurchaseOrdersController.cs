using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiningOps.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        private readonly AppDbContext _context;

        public PurchaseOrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseOrders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.PurchaseOrdersDb
                .Include(o => o.Supplier)
                .Include(o => o.Requester)
                .Include(o => o.MaterialRequest)
                .ToListAsync();

            return View(orders);
        }

        // GET: PurchaseOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.PurchaseOrdersDb
                .Include(o => o.Supplier)
                .Include(o => o.Requester)
                .Include(o => o.MaterialRequest)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // GET: PurchaseOrders/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: PurchaseOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = new PurchaseOrder
                {
                    SupplierId = model.SupplierId,
                    RequestedBy = model.RequestedBy,
                    Currency = string.IsNullOrWhiteSpace(model.Currency) ? "ZAR" : model.Currency.Trim(),
                    Status = model.Status,
                    ExpectedDeliveryDate = model.ExpectedDeliveryDate,
                    TotalAmount = model.TotalAmount,
                    MaterialRequestId = model.MaterialRequestId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Add(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Purchase order created successfully!";
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(model);
            return View(model);
        }

        // GET: PurchaseOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.PurchaseOrdersDb.FindAsync(id);
            if (order == null) return NotFound();

            var model = new PurchaseOrderViewModel
            {
                SupplierId = order.SupplierId,
                RequestedBy = order.RequestedBy,
                Currency = order.Currency,
                Status = order.Status,
                ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                TotalAmount = order.TotalAmount,
                MaterialRequestId = order.MaterialRequestId
            };

            PopulateDropdowns(model);
            return View(model);
        }

        // POST: PurchaseOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PurchaseOrderViewModel model)
        {
            if (!await _context.PurchaseOrdersDb.AnyAsync(p => p.OrderId == id))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var order = await _context.PurchaseOrdersDb.FindAsync(id);
                    if (order == null) return NotFound();

                    order.SupplierId = model.SupplierId;
                    order.RequestedBy = model.RequestedBy;
                    order.Currency = model.Currency;
                    order.Status = model.Status;
                    order.ExpectedDeliveryDate = model.ExpectedDeliveryDate;
                    order.TotalAmount = model.TotalAmount;
                    order.MaterialRequestId = model.MaterialRequestId;

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Purchase order updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.PurchaseOrdersDb.AnyAsync(e => e.OrderId == id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(model);
            return View(model);
        }

        // GET: PurchaseOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.PurchaseOrdersDb
                .Include(o => o.Supplier)
                .Include(o => o.Requester)
                .Include(o => o.MaterialRequest)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.PurchaseOrdersDb.FindAsync(id);
            if (order != null)
            {
                _context.PurchaseOrdersDb.Remove(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Purchase order deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Populate dropdowns safely, with role-based supplier filtering
        /// </summary>
        private void PopulateDropdowns(PurchaseOrderViewModel? model = null)
        {
            // Retrieve role and supplier claims
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == "UserRole")?.Value;
            var supplierClaim = User.Claims.FirstOrDefault(c => c.Type == "SupplierId")?.Value;

            UserRole role = Enum.TryParse(roleClaim, out UserRole parsedRole) ? parsedRole : UserRole.Supervisor;
            int? currentSupplierId = supplierClaim != null ? int.Parse(supplierClaim) : null;

            // Suppliers
            var suppliers = _context.SupplierProfiles.AsQueryable();
            if (role == UserRole.Supplier && currentSupplierId.HasValue)
            {
                suppliers = suppliers.Where(s => s.SupplierId == currentSupplierId.Value);
            }

            ViewBag.Supplier = suppliers
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.CompanyName,
                    Selected = model != null && model.SupplierId == s.SupplierId
                })
                .ToList();

            // Requesters
            ViewBag.RequestedBy = _context.RegisterMiningDb
                .Select(r => new SelectListItem
                {
                    Value = r.AccId.ToString(),
                    Text = r.FullName,
                    Selected = model != null && model.RequestedBy == r.AccId
                })
                .ToList();

            // Material Requests
            ViewBag.MaterialRequestId = _context.MaterialRequestsDb
                .Select(m => new SelectListItem
                {
                    Value = m.MaterialRequestId.ToString(),
                    Text = m.ItemName,
                    Selected = model != null && model.MaterialRequestId == m.MaterialRequestId
                })
                .ToList();
        }
    }
}
