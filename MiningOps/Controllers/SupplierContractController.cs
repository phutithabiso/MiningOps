using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MiningOps.Controllers
{
    public class SupplierContractsController : Controller
    {
        private readonly AppDbContext _context;

        public SupplierContractsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SupplierContracts
        public async Task<IActionResult> Index()
        {
            var contracts = await _context.SupplierContractDb
                .Include(c => c.Supplier) 
                .ToListAsync();

            return View(contracts);
        }


        // GET: SupplierContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var contract = await _context.SupplierContractDb
                .Include(c => c.Supplier)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (contract == null)
                return NotFound();

            return View(contract);
        }

        // GET: SupplierContracts/Create
        public IActionResult Create()
        {
            ViewData["Suppliers"] = _context.SupplierProfiles.ToList() ?? new List<Supplier>();
            return View();
        }

        // POST: SupplierContracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierContractViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contract = new SupplierContract
                {
                    SupplierId = model.SupplierId,
                    ContractTerms = model.ContractTerms,
                    PaymentTerms = model.PaymentTerms,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ContractValue = model.ContractValue,
                    ContractType =model.ContractType

                };

                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Suppliers"] = _context.SupplierProfiles.ToList() ?? new List<Supplier>();
           

            return View(model);
        }

        // GET: SupplierContracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var contract = await _context.SupplierContractDb.FindAsync(id);
            if (contract == null)
                return NotFound();

            var model = new SupplierContractViewModel
            {
           
                SupplierId = contract.SupplierId,
                ContractTerms = contract.ContractTerms,
                PaymentTerms = contract.PaymentTerms,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                ContractValue = contract.ContractValue ?? 0m,
                ContractType = contract.ContractType
            };

           // ViewData["Suppliers"] = _context.SupplierProfiles.ToList();
            ViewData["Suppliers"] = _context.SupplierProfiles.ToList() ?? new List<Supplier>();

            return View(model);
        }

        // POST: SupplierContracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierContractViewModel model)
        {
            if (id != model.SupplierId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var contract = await _context.SupplierContractDb.FindAsync(id);
                    if (contract == null)
                        return NotFound();
                    
                    contract.SupplierId = model.SupplierId;
                    contract.ContractTerms = model.ContractTerms;
                    contract.PaymentTerms = model.PaymentTerms;
                    contract.StartDate = model.StartDate;
                    contract.EndDate = model.EndDate;
                    contract.ContractValue = model.ContractValue;
                    contract.ContractType = model.ContractType;
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SupplierContractDb.Any(e => e.ContractId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Suppliers"] = _context.SupplierProfiles.ToList();
            return View(model);
        }

        // GET: SupplierContracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var contract = await _context.SupplierContractDb
                .Include(c => c.Supplier)
                .FirstOrDefaultAsync(m => m.ContractId == id);
            if (contract == null)
                return NotFound();

            return View(contract);
        }

        // POST: SupplierContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.SupplierContractDb.FindAsync(id);
            if (contract != null)
            {
                _context.SupplierContractDb.Remove(contract);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
