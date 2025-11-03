using Microsoft.AspNetCore.Mvc;
using MiningOps.Services;
namespace MiningOps.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardAnalyticsService _dashboardService;

        public DashboardController(DashboardAnalyticsService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> AdminDashboard()
        {
            var data = await _dashboardService.GetAdminDashboardAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> SupplierDashboard(int supplierId)
        {
            var data = await _dashboardService.GetSupplierDashboardAsync(supplierId);
            return View(data);
        }
    }
}

