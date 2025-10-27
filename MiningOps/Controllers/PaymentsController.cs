using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MiningOps.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly AppDbContext _context;

        public PaymentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var payments = await _context.PaymentsDb
                .Include(p => p.Invoice)
                .Include(p => p.ApprovedBy)
                .ToListAsync();

            return View(payments);
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.PaymentsDb
                .Include(p => p.Invoice)
                .Include(p => p.ApprovedBy)
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment == null) return NotFound();

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var payment = new Payment
                {
                    InvoiceId = model.InvoiceId,
                    Amount = model.Amount,
                    PaidDate = model.PaidDate,
                    ApprovedById = model.ApprovedById,
                    PaymentReference = model.PaymentReference
                };

                _context.Add(payment);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Payment created successfully!";
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(model);
            return View(model);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.PaymentsDb
                .Include(p => p.Invoice)
                .Include(p => p.ApprovedBy)
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment == null) return NotFound();

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.PaymentsDb.FindAsync(id);
            if (payment != null)
            {
                _context.PaymentsDb.Remove(payment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Payment deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Populate dropdowns for Invoice and ApprovedBy (Admin)
        /// </summary>
        private void PopulateDropdowns(PaymentViewModel? model = null)
        {
            // Invoice dropdown
            ViewBag.InvoiceId = _context.InvoicesDb
                .Select(i => new SelectListItem
                {
                    Value = i.InvoiceId.ToString(),
                    Text = $"Invoice #{i.InvoiceId} - {i.Amount:C} ({i.Status})",
                    Selected = model != null && model.InvoiceId == i.InvoiceId
                })
                .ToList();

            // Admin dropdown
            ViewBag.ApprovedById = _context.RegisterMiningDb
                .Select(a => new SelectListItem
                {
                    Value = a.AccId.ToString(),
                    Text = a.FullName,
                    Selected = model != null && model.ApprovedById == a.AccId
                })
                .ToList();
        }
    }
}
