using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;

namespace MiningOps.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly AppDbContext _context;

        public UserManagementController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserManagement
        public async Task<IActionResult> Index()
        {
            // Include role-specific profiles
            var users = await _context.RegisterMiningDb
                .Include(u => u.AdminProfile)
                .Include(u => u.SupervisorProfile)
                .Include(u => u.SupplierProfile)
                .ToListAsync();

            return View(users);
        }

        // GET: UserManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.RegisterMiningDb
                .Include(u => u.AdminProfile)
                .Include(u => u.SupervisorProfile)
                .Include(u => u.SupplierProfile)
                .FirstOrDefaultAsync(m => m.AccId == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: UserManagement/Create
        public IActionResult Create()
        {
            ViewData["Roles"] = Enum.GetValues(typeof(UserRole)).Cast<UserRole>();
            return View();
        }

        // POST: UserManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterMining user)
        {
            if (ModelState.IsValid)
            {
                // Add the user
                _context.Add(user);
                await _context.SaveChangesAsync();

                // Add role-specific profile
                switch (user.Role)
                {
                    case UserRole.Admin:
                        _context.AdminProfiles.Add(new Admin
                        {
                            
                            AccId = user.AccId,
                            Department = "IT"
                        });
                        break;

                    case UserRole.Supervisor:
                        _context.SupervisorProfiles.Add(new Supervisor
                        {
                            AccId = user.AccId,
                            Team = "Operations",
                            MineLocation = "Default Mine",
                            Shift = "Day"
                        });
                        break;

                    case UserRole.Supplier:
                        _context.SupplierProfiles.Add(new Supplier
                        {
                            AccId = user.AccId,
                            CompanyName = "New Supplier",
                            ContactPerson = user.FullName,
                            Address = "Default Address"
                        });
                        break;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Roles"] = Enum.GetValues(typeof(UserRole)).Cast<UserRole>();
            return View(user);
        }

        // GET: UserManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.RegisterMiningDb
                .Include(u => u.AdminProfile)
                .Include(u => u.SupervisorProfile)
                .Include(u => u.SupplierProfile)
                .FirstOrDefaultAsync(u => u.AccId == id);

            if (user == null) return NotFound();

            ViewData["Roles"] = Enum.GetValues(typeof(UserRole)).Cast<UserRole>();
            return View(user);
        }

        // POST: UserManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RegisterMining user)
        {
            if (id != user.AccId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);

                    // Update role-specific profiles if needed
                    switch (user.Role)
                    {
                        case UserRole.Admin:
                            var adminProfile = await _context.AdminProfiles.FindAsync(user.AccId);
                            if (adminProfile != null) _context.Update(adminProfile);
                            break;

                        case UserRole.Supervisor:
                            var supervisorProfile = await _context.SupervisorProfiles.FindAsync(user.AccId);
                            if (supervisorProfile != null) _context.Update(supervisorProfile);
                            break;

                        case UserRole.Supplier:
                            var supplierProfile = await _context.SupplierProfiles.FindAsync(user.AccId);
                            if (supplierProfile != null) _context.Update(supplierProfile);
                            break;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.RegisterMiningDb.Any(e => e.AccId == user.AccId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["Roles"] = Enum.GetValues(typeof(UserRole)).Cast<UserRole>();
            return View(user);
        }

        // GET: UserManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.RegisterMiningDb
                .Include(u => u.AdminProfile)
                .Include(u => u.SupervisorProfile)
                .Include(u => u.SupplierProfile)
                .FirstOrDefaultAsync(u => u.AccId == id);

            if (user == null) return NotFound();

            return View(user);
        }

        // POST: UserManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.RegisterMiningDb.FindAsync(id);
            if (user != null)
            {
                // Delete role-specific profile
                switch (user.Role)
                {
                    case UserRole.Admin:
                        var adminProfile = await _context.AdminProfiles.FindAsync(id);
                        if (adminProfile != null) _context.AdminProfiles.Remove(adminProfile);
                        break;

                    case UserRole.Supervisor:
                        var supervisorProfile = await _context.SupervisorProfiles.FindAsync(id);
                        if (supervisorProfile != null) _context.SupervisorProfiles.Remove(supervisorProfile);
                        break;

                    case UserRole.Supplier:
                        var supplierProfile = await _context.SupplierProfiles.FindAsync(id);
                        if (supplierProfile != null) _context.SupplierProfiles.Remove(supplierProfile);
                        break;
                }

                _context.RegisterMiningDb.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}