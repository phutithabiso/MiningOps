using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models; 
using System.Linq;

namespace MiningOps.Controllers
{
    public class SupplierPerformanceController : Controller
    {
        private readonly AppDbContext _context;

        public SupplierPerformanceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SupplierPerformance
        public IActionResult Index()
        {
            var performances = _context.SupplierPerformanceDb
                .Include(p => p.Supplier)
                .ToList();
            return View(performances);
        }

        // GET: SupplierPerformance/Create
        public IActionResult Create()
        {
            var suppliers = _context.SupplierProfiles
                            .Select(s => new SelectListItem
                            {
                                Value = s.SupplierId.ToString(),
                                Text = s.CompanyName
                            })
                            .ToList();

            var viewModel = new SupplierPerformanceViewModel
            {
                Suppliers = suppliers
            };

            ViewBag.SupplierId = suppliers; // Optional if using ViewBag
            return View(viewModel);
        }

        // POST: SupplierPerformance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SupplierPerformanceViewModel performance)
        {
            if (ModelState.IsValid)
            {
                var content = new SupplierPerformance()
                {
                    SupplierId = performance.SupplierId,
                    OnTimeDeliveryRate = performance.OnTimeDeliveryRate,
                    QualityRating = performance.QualityRating,
                    ComplianceScore = performance.ComplianceScore,
                    ReportDate = performance.ReportDate
                };

                _context.SupplierPerformanceDb.Add(content);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Suppliers = _context.SupplierProfiles.ToList();
            return View(performance);
        }

        // GET: SupplierPerformance/Edit/5
        public IActionResult Edit(int id)
        {
            var performance = _context.SupplierPerformanceDb.Find(id);
            if (performance == null)
            {
                return NotFound();
            }

            ViewBag.Suppliers = _context.SupplierProfiles.ToList();
            return View(performance);
        }

        // POST: SupplierPerformance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SupplierPerformance performance)
        {
            if (id != performance.PerformanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.SupplierPerformanceDb.Update(performance);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Suppliers = _context.SupplierProfiles.ToList();
            return View(performance);
        }

        // GET: SupplierPerformance/Delete/5
        public IActionResult Delete(int id)
        {
            var performance = _context.SupplierPerformanceDb
                .Include(p => p.Supplier)
                .FirstOrDefault(p => p.PerformanceId == id);

            if (performance == null)
            {
                return NotFound();
            }

            _context.SupplierPerformanceDb.Remove(performance);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: SupplierPerformance/Details/5
        public IActionResult Details(int id)
        {
            var performance = _context.SupplierPerformanceDb
                .Include(p => p.Supplier)
                .FirstOrDefault(p => p.PerformanceId == id);

            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }
    }
}
