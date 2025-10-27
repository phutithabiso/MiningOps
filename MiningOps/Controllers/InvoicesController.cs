using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiningOps.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly AppDbContext _context;

        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.InvoicesDb
                .Include(i => i.Order)
                .ThenInclude(o => o.Supplier)
                .ToListAsync();

            return View(invoices);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.InvoicesDb
                .Include(i => i.Order)
                .ThenInclude(o => o.Supplier)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null) return NotFound();

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var invoice = new Invoice
                {
                    OrderId = model.OrderId,
                    InvoiceDate = model.InvoiceDate,
                    DueDate = model.DueDate,
                    Amount = model.Amount,
                    Status = model.Status,
                    InvoiceReference = model.InvoiceReference,
                    InvoiceFilePath = model.InvoiceFilePath
                };

                _context.Add(invoice);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Invoice created successfully!";
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(model);
            return View(model);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.InvoicesDb.FindAsync(id);
            if (invoice == null) return NotFound();

            var model = new InvoiceViewModel
            {
                InvoiceId = invoice.InvoiceId,
                OrderId = invoice.OrderId,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                Amount = invoice.Amount,
                Status = invoice.Status,
                InvoiceReference = invoice.InvoiceReference,
                InvoiceFilePath = invoice.InvoiceFilePath
            };

            PopulateDropdowns(model);
            return View(model);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InvoiceViewModel model)
        {
            if (id != model.InvoiceId) return NotFound();

            if (ModelState.IsValid)
            {
                var invoice = await _context.InvoicesDb.FindAsync(id);
                if (invoice == null) return NotFound();

                invoice.OrderId = model.OrderId;
                invoice.InvoiceDate = model.InvoiceDate;
                invoice.DueDate = model.DueDate;
                invoice.Amount = model.Amount;
                invoice.Status = model.Status;
                invoice.InvoiceReference = model.InvoiceReference;
                invoice.InvoiceFilePath = model.InvoiceFilePath;

                _context.Update(invoice);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Invoice updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(model);
            return View(model);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.InvoicesDb
                .Include(i => i.Order)
                .ThenInclude(o => o.Supplier)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null) return NotFound();

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.InvoicesDb.FindAsync(id);
            if (invoice != null)
            {
                _context.InvoicesDb.Remove(invoice);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Invoice deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Populate dropdowns for PurchaseOrders and InvoiceStatus
        private void PopulateDropdowns(InvoiceViewModel? model = null)
        {
            // Purchase Orders
            ViewBag.OrderId = _context.PurchaseOrdersDb
                .Include(o => o.Supplier)
                .Select(o => new SelectListItem
                {
                    Value = o.OrderId.ToString(),
                    Text = $"PO#{o.OrderId} - {o.Supplier.CompanyName}",
                    Selected = model != null && model.OrderId == o.OrderId
                })
                .ToList();

            // Invoice Status
            ViewBag.StatusList = Enum.GetValues(typeof(MiningOps.Entity.roleFolder.InvoiceStatus))
                .Cast<MiningOps.Entity.roleFolder.InvoiceStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString(),
                    Selected = model != null && model.Status == s
                }).ToList();
        }
    }
}
