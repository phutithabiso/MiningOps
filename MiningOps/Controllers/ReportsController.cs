using Microsoft.AspNetCore.Mvc;
using MiningOps.Services;
using System.Threading.Tasks;

namespace MiningOps.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        // Export Payments Report
        [HttpGet]
        public async Task<IActionResult> ExportPaymentsReport()
        {
            var fileBytes = await _reportService.GeneratePaymentsReportAsync();
            return File(fileBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "PaymentsReport.xlsx");
        }

        // Export MiningOps full report (Payments, Invoices, Materials)
        [HttpGet]
        public async Task<IActionResult> ExportMiningOpsReport()
        {
            var fileBytes = await _reportService.GenerateMiningOpsReportAsync();
            return File(fileBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "MiningOpsReport.xlsx");
        }
    }
}
