using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MiningOps.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly AppDbContext _context;

        public AdminDashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Payments chart data (monthly total)
            var paymentsData = await _context.PaymentsDb
                .GroupBy(p => p.PaidDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(p => p.Amount)
                }).ToListAsync();

            ViewBag.PaymentLabels = paymentsData.Select(p => $"Month {p.Month}").ToList();
            ViewBag.PaymentValues = paymentsData.Select(p => p.Total).ToList();

            // Invoice status data
            var invoiceStatus = await _context.InvoicesDb
                .GroupBy(i => i.Status)
                .Select(g => new
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                }).ToListAsync();

            ViewBag.InvoiceLabels = invoiceStatus.Select(i => i.Status).ToList();
            ViewBag.InvoiceValues = invoiceStatus.Select(i => i.Count).ToList();

            // Material requests data
            var materialRequests = await _context.MaterialRequestsDb
                .GroupBy(m => m.ItemName)
                .Select(g => new
                {
                    Item = g.Key,
                    Quantity = g.Sum(m => m.Quantity)
                }).ToListAsync();

            ViewBag.MaterialLabels = materialRequests.Select(m => m.Item).ToList();
            ViewBag.MaterialValues = materialRequests.Select(m => m.Quantity).ToList();

            return View();
        }
    }
}
