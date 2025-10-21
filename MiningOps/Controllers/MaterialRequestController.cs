using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MiningOps.Controllers
{
    public class MaterialRequestController : Controller
    {
        private readonly AppDbContext _context;

        public MaterialRequestController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.MaterialRequestsDb
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
            return View(requests);
        }

        public IActionResult Create()
        {
            return View(new MaterialRequestViewModel
            {
                RequestDate = DateTime.Now
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = new MaterialRequest
                {
                    ItemName = model.ItemName,
                    Quantity = model.Quantity,
                    Status = model.Status,
                    RequestDate = model.RequestDate
                };

                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var request = await _context.MaterialRequestsDb.FindAsync(id);
            if (request == null) return NotFound();

            var model = new MaterialRequestViewModel
            {
                MaterialRequestId = request.MaterialRequestId,
                ItemName = request.ItemName,
                Quantity = request.Quantity,
                Status = request.Status,
                RequestDate = request.RequestDate
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialRequestViewModel model)
        {
            if (id != model.MaterialRequestId) return NotFound();

            if (ModelState.IsValid)
            {
                var request = await _context.MaterialRequestsDb.FindAsync(id);
                if (request == null) return NotFound();

                request.ItemName = model.ItemName;
                request.Quantity = model.Quantity;
                request.Status = model.Status;
                request.RequestDate = model.RequestDate;

                _context.Update(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var request = await _context.MaterialRequestsDb
                .FirstOrDefaultAsync(r => r.MaterialRequestId == id);

            if (request == null) return NotFound();
            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.MaterialRequestsDb.FindAsync(id);
            if (request != null)
            {
                _context.MaterialRequestsDb.Remove(request);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
