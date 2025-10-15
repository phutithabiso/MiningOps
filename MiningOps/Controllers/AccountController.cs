using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiningOps.Entity;
using MiningOps.Models;
using MiningOps.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MiningOps.Controllers
{
    public class AccountController : Controller
    {
            public readonly AppDbContext _context;
            public AccountController(AppDbContext appDbContext)
            {
                // Constructor logic if needed
                _context = appDbContext;
            }
            public IActionResult Login()
            {
                return View();
            }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
               
                var usr = _context.RegisterMiningDb
                    .FirstOrDefault(x => x.Username == user.usernameoremail || x.Email == user.usernameoremail);

                if (usr != null)
                {
      
                    bool isPasswordValid = PasswordHasher.VerifyPassword(user.password, usr.PasswordHash, usr.Salt);


                    if (isPasswordValid)
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usr.AccId.ToString()),
                    new Claim(ClaimTypes.Email, usr.Email ?? string.Empty),
                    new Claim(ClaimTypes.Name, usr.Username),
                    new Claim("FullName", usr.FullName ?? ""),
                    new Claim(ClaimTypes.Role, usr.Role.ToString())
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity)
                        );

                        return RedirectToAction("Dashboard", "Account");
                    }
                }

                ModelState.AddModelError("", "Invalid username or password");
            }

            return View(user);
        }


        public IActionResult Register()
            {
                return View();
            }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                //  Check for duplicate username/email
                if (_context.RegisterMiningDb.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username already taken.");
                    return View(model);
                }

                if (_context.RegisterMiningDb.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email already registered.");
                    return View(model);
                }

                // Validate Role
                if (!Enum.IsDefined(typeof(UserRole), model.Role))
                {
                    ModelState.AddModelError("Role", "Invalid role selected.");
                    return View(model);
                }

                //  Validate password
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    ModelState.AddModelError("Password", "Password cannot be empty.");
                    return View(model);
                }

                //  Hash password with salt
                string salt = PasswordHasher.GenerateSalt();
                string hashedPassword = PasswordHasher.HashPassword(model.Password, salt);

                // 5Create user entity
                var acc = new RegisterMining
                {
                    FullName = model.FullName?.Trim(),
                    Username = model.Username?.Trim(),
                    Email = model.Email?.Trim(),
                    PhoneNumber = model.PhoneNumber?.Trim(),
                    Salt = salt,
                    PasswordHash = hashedPassword,
                    Role = model.Role,
                    CreatedAt = DateTime.UtcNow
                };

                //  Save user
                _context.RegisterMiningDb.Add(acc);
                _context.SaveChanges();

                // 7️⃣ Redirect based on role
                return acc.Role switch
                {
                    UserRole.Admin => RedirectToAction("CompleteAdminProfile", new { id = acc.AccId }),
                    UserRole.Supervisor => RedirectToAction("CompleteSupervisorProfile", new { id = acc.AccId }),
                    UserRole.Supplier => RedirectToAction("CompleteSupplierProfile", new { id = acc.AccId }),
                    _ => RedirectToAction("Login")
                };
            }
            catch (DbUpdateException dbEx)
            {
                // Handles database-specific errors (e.g., constraints)
                Console.WriteLine(dbEx.ToString());
                ModelState.AddModelError("", $"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                // Handles any other errors
                Console.WriteLine(ex.ToString());
                ModelState.AddModelError("", $"Unexpected error: {ex.Message}");
            }

            return View(model);
        }


        public IActionResult Logout()
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                // Logic for logging out the user
                return RedirectToAction("Login");
            }
            public IActionResult Index()
            {
                // Logic to display the index page
                return View(_context.RegisterMiningDb.ToList());
            }
            public IActionResult Profile()
            { 

                return View();
            }
        [Authorize]
            public IActionResult Dashboard()
            {

                //ViewBag.Name = HttpContext.User.Identity?.Name;

                return View();
            }
            public IActionResult Delete(int id)
            {
                if (id == null)
                {
                    return NotFound();
                }
                var user = _context.RegisterMiningDb.FirstOrDefault(x => x.AccId == id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);

            }
        // Admin continuation
        public IActionResult CompleteAdminProfile(int id)
        {
           var model =new AdminViewModel { AccId = id};
            return View(model);
        }

        [HttpPost]
        public IActionResult CompleteAdminProfile(int id, AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.RegisterMiningDb.FirstOrDefault(x => x.AccId == id);
                if (user == null) return NotFound();

                var profile = new Admin
                {
                    AccId = id,
                    Department = model.Department,
                    CanManageUsers = model.CanManageUsers,
                    CanApproveRequests = model.CanApproveRequests
                };

                _context.AdminProfiles.Add(profile);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Admin profile completed!";
                return RedirectToAction("Index", "UserManagement");
            }
            return View(model);
        }


        // Supervisor continuation
        //public IActionResult CompleteSupervisorProfile(int id)
        //{
        //    return View(new SupervisorViewModel { });
        //}
        public IActionResult CompleteSupervisorProfile(int id)
        {
            var model = new SupervisorViewModel { AccId = id };
            return View(model); 
        }
        [HttpPost]
        public IActionResult CompleteSupervisorProfile(SupervisorViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _context.RegisterMiningDb.FirstOrDefault(x => x.AccId == model.AccId);
            if (user == null) return NotFound();

            var profile = new Supervisor
            {
                AccId = user.AccId,
                Team = model.Team,
                MineLocation = model.MineLocation,
                Shift = model.Shift,
                CanViewReports = model.CanViewReports,
                CanManageTasks = model.CanManageTasks,
                RegisterMining = user
            };

            _context.SupervisorProfiles.Add(profile);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Supervisor profile for {user.FullName} completed!";
            return RedirectToAction("Index", "UserManagement");
        }


        // Supplier continuation
        public IActionResult CompleteSupplierProfile(int id)
        {
            var model = new SupplierViewModel { AccId = id };
            return View(model);
        }

        [HttpPost]
        public IActionResult CompleteSupplierProfile(int id, SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.RegisterMiningDb.FirstOrDefault(x => x.AccId == id);
                if (user == null) return NotFound();

                var profile = new Supplier
                {
                    AccId = id,
                    CompanyName = model.CompanyName,
                    ContactPerson = model.ContactPerson,
                    Address = model.Address,
                    CanViewOrders = model.CanViewOrders,
                    CanManageInventory = model.CanManageInventory
                };

                _context.SupplierProfiles.Add(profile);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Supplier profile completed!";
                return RedirectToAction("Index", "UserManagement");
            }
            return View(model);
        }

        // GET: Account/EditAdminProfile/5
        public IActionResult EditAdminProfile(int id)
        {
            var profile = _context.AdminProfiles.FirstOrDefault(a => a.AccId == id);
            if (profile == null) return NotFound();

            var model = new AdminViewModel
            {
                AccId = id,
                Department = profile.Department,
                CanManageUsers = profile.CanManageUsers,
                CanApproveRequests = profile.CanApproveRequests
            };

            return View(model); // Create EditAdminProfile.cshtml
        }

        // POST: Account/EditAdminProfile/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAdminProfile(int id, AdminViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var profile = _context.AdminProfiles.FirstOrDefault(a => a.AccId == id);
            if (profile == null) return NotFound();

            profile.Department = model.Department;
            profile.CanManageUsers = model.CanManageUsers;
            profile.CanApproveRequests = model.CanApproveRequests;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Admin profile updated!";

            return RedirectToAction("Index", "UserManagement");
        }


        // Supervisor profile edit
        public IActionResult EditSupervisorProfile(int id)
        {
            var profile = _context.SupervisorProfiles.FirstOrDefault(s => s.AccId == id);
            if (profile == null) return NotFound();

            var model = new SupervisorViewModel
            {
                AccId = id,
                Team = profile.Team,
                MineLocation = profile.MineLocation,
                Shift = profile.Shift,
                CanViewReports = profile.CanViewReports,
                CanManageTasks = profile.CanManageTasks
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditSupervisorProfile(SupervisorViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var profile = _context.SupervisorProfiles.FirstOrDefault(s => s.AccId == model.AccId);
            if (profile == null) return NotFound();

            profile.Team = model.Team;
            profile.MineLocation = model.MineLocation;
            profile.Shift = model.Shift;
            profile.CanViewReports = model.CanViewReports;
            profile.CanManageTasks = model.CanManageTasks;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Supervisor profile updated!";
            return RedirectToAction("Index", "UserManagement");
        }

        // Supplier profile edit
        public IActionResult EditSupplierProfile(int id)
        {
            var profile = _context.SupplierProfiles.FirstOrDefault(s => s.AccId == id);
            if (profile == null) return NotFound();

            var model = new SupplierViewModel
            {
                AccId = id,
                CompanyName = profile.CompanyName,
                ContactPerson = profile.ContactPerson,
                Address = profile.Address,
                CanViewOrders = profile.CanViewOrders,
                CanManageInventory = profile.CanManageInventory
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditSupplierProfile(int id, SupplierViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var profile = _context.SupplierProfiles.FirstOrDefault(s => s.AccId == id);
            if (profile == null) return NotFound();

            profile.CompanyName = model.CompanyName;
            profile.ContactPerson = model.ContactPerson;
            profile.Address = model.Address;
            profile.CanViewOrders = model.CanViewOrders;
            profile.CanManageInventory = model.CanManageInventory;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Supplier profile updated!";
            return RedirectToAction("Index", "UserManagement");
        }


    }
}
