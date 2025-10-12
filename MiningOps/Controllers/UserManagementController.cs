using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models;
using MiningOps.Security;

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
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = Enum.GetValues(typeof(UserRole)).Cast<UserRole>();
                return View(model);
            }

            // Check duplicates
            if (_context.RegisterMiningDb.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }
            if (_context.RegisterMiningDb.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            // Hash password
            string salt = PasswordHasher.GenerateSalt();
            string hashedPassword = PasswordHasher.HashPassword(model.Password, salt);

            // Map to entity
            var user = new RegisterMining
            {
                FullName = model.FullName.Trim(),
                Username = model.Username.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                PasswordHash = hashedPassword,
                Salt = salt,
                Role = model.Role,
                CreatedAt = DateTime.UtcNow
            };

            // Save entity to generate AccId
            _context.RegisterMiningDb.Add(user);
            await _context.SaveChangesAsync();

            // Redirect based on role
            return user.Role switch
            {
                UserRole.Admin => RedirectToAction("CompleteAdminProfile", "Account", new { id = user.AccId }),
                UserRole.Supervisor => RedirectToAction("CompleteSupervisorProfile", "Account", new { id = user.AccId }),
                UserRole.Supplier => RedirectToAction("CompleteSupplierProfile", "Account", new { id = user.AccId }),
                _ => RedirectToAction("Index", "UserManagement")
            };
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