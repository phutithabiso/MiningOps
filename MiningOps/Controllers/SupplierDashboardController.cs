using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models; 
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiningOps.Controllers
{
    public class SupplierDashboardController : Controller
    {
        private readonly AppDbContext _context;

        public SupplierDashboardController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SupplierDashboard
        public async Task<IActionResult> Index()
        {
            // Load suppliers with contracts and performance
            var suppliers = await _context.SupplierProfiles
                .Include(s => s.Contracts)
                .Include(s => s.Performances)
                .ToListAsync();

            var dashboardList = suppliers.Select(s =>
            {
                var contracts = s.Contracts ?? new List<SupplierContract>();
                var performance = s.Performances ?? new List<SupplierPerformance>();

                return new SupplierDashboardViewModel
                {
                    SupId = s.SupplierId,
                    SupplierName = s.CompanyName,
                    ContactPerson = s.ContactPerson,

                    SupplierContracts = contracts,
                    SupplierPerformance = performance,

                    ActiveContractCount = contracts.Count(c => c.EndDate.HasValue && c.EndDate.Value >= DateTime.Now),
                    ExpiringContractCount = contracts.Count(c =>
                        c.EndDate.HasValue &&
                        c.EndDate.Value < DateTime.Now.AddDays(30) &&
                        c.EndDate.Value >= DateTime.Now),

                    AverageQualityRating = performance.Any()
                        ? Math.Round(performance.Average(p => (decimal?)p.QualityRating) ?? 0, 2)
                        : 0,

                    AverageDeliveryRate = performance.Any()
                        ? Math.Round(performance.Average(p => (decimal?)p.OnTimeDeliveryRate) ?? 0, 2)
                        : 0
                };
            }).ToList();

            return View(dashboardList);
        }


        // GET: SupplierDashboard/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _context.SupplierProfiles
                .Include(s => s.Contracts)
                .Include(s => s.Performances)
                .FirstOrDefaultAsync(s => s.SupplierId == id);

            if (supplier == null)
                return NotFound();

            var model = new SupplierDashboardViewModel
            {
                SupId = supplier.SupplierId,
                SupplierName = supplier.CompanyName,
                ContactPerson = supplier.ContactPerson,
                SupplierContracts = supplier.Contracts,
                SupplierPerformance = supplier.Performances
            };

            return View(model);
        }
    }
}
