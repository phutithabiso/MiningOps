using Microsoft.AspNetCore.Mvc;

namespace MiningOps.Controllers
{
    public class InvetoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
