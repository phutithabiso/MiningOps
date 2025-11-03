using MiningOps.Entity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;

namespace MiningOps.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Generate Payments Report (Excel)
        /// </summary>
        public async Task<byte[]> GeneratePaymentsReportAsync()
        {
            var payments = await _context.PaymentsDb
                .Include(p => p.Invoice)
                .ThenInclude(i => i.Order)
                .ToListAsync();

            // Create ExcelPackage (license already set in Program.cs)
            using var package = new ExcelPackage(new MemoryStream());
            var ws = package.Workbook.Worksheets.Add("Payments");

            // Header
            ws.Cells[1, 1].Value = "Payment ID";
            ws.Cells[1, 2].Value = "Invoice Reference";
            ws.Cells[1, 3].Value = "Amount";
            ws.Cells[1, 4].Value = "Paid Date";

            int row = 2;
            foreach (var p in payments)
            {
                ws.Cells[row, 1].Value = p.PaymentId;
                ws.Cells[row, 2].Value = p.Invoice?.InvoiceReference;
                ws.Cells[row, 3].Value = p.Amount;
                ws.Cells[row, 4].Value = p.PaidDate.ToString("yyyy-MM-dd");
                row++;
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            return package.GetAsByteArray();
        }

        /// <summary>
        /// Generate MiningOps report with multiple sheets
        /// </summary>
        public async Task<byte[]> GenerateMiningOpsReportAsync()
        {
            using var package = new ExcelPackage(new MemoryStream());

            // --- Payments Sheet ---
            var wsPayments = package.Workbook.Worksheets.Add("Payments");
            var payments = await _context.PaymentsDb
                .Include(p => p.Invoice)
                .ThenInclude(i => i.Order)
                .ToListAsync();

            wsPayments.Cells[1, 1].Value = "Payment ID";
            wsPayments.Cells[1, 2].Value = "Invoice Reference";
            wsPayments.Cells[1, 3].Value = "Amount";
            wsPayments.Cells[1, 4].Value = "Paid Date";

            int row = 2;
            foreach (var p in payments)
            {
                wsPayments.Cells[row, 1].Value = p.PaymentId;
                wsPayments.Cells[row, 2].Value = p.Invoice?.InvoiceReference;
                wsPayments.Cells[row, 3].Value = p.Amount;
                wsPayments.Cells[row, 4].Value = p.PaidDate.ToString("yyyy-MM-dd");
                row++;
            }
            wsPayments.Cells[wsPayments.Dimension.Address].AutoFitColumns();

            // --- Invoices Sheet ---
            var wsInvoices = package.Workbook.Worksheets.Add("Invoices");
            var invoices = await _context.InvoicesDb.Include(i => i.Order).ToListAsync();

            wsInvoices.Cells[1, 1].Value = "Invoice ID";
            wsInvoices.Cells[1, 2].Value = "Order ID";
            wsInvoices.Cells[1, 3].Value = "Amount";
            wsInvoices.Cells[1, 4].Value = "Status";
            wsInvoices.Cells[1, 5].Value = "Invoice Date";

            row = 2;
            foreach (var i in invoices)
            {
                wsInvoices.Cells[row, 1].Value = i.InvoiceId;
                wsInvoices.Cells[row, 2].Value = i.OrderId;
                wsInvoices.Cells[row, 3].Value = i.Amount;
                wsInvoices.Cells[row, 4].Value = i.Status.ToString();
                wsInvoices.Cells[row, 5].Value = i.InvoiceDate.ToString("yyyy-MM-dd");
                row++;
            }
            wsInvoices.Cells[wsInvoices.Dimension.Address].AutoFitColumns();

            // --- Material Requests Sheet ---
            var wsMaterials = package.Workbook.Worksheets.Add("Materials");
            var materials = await _context.MaterialRequestsDb.ToListAsync();

            wsMaterials.Cells[1, 1].Value = "Material Request ID";
            wsMaterials.Cells[1, 2].Value = "Item Name";
            wsMaterials.Cells[1, 3].Value = "Quantity";
            wsMaterials.Cells[1, 4].Value = "Status";
            wsMaterials.Cells[1, 5].Value = "Request Date";

            row = 2;
            foreach (var m in materials)
            {
                wsMaterials.Cells[row, 1].Value = m.MaterialRequestId;
                wsMaterials.Cells[row, 2].Value = m.ItemName;
                wsMaterials.Cells[row, 3].Value = m.Quantity;
                wsMaterials.Cells[row, 4].Value = m.Status;
                wsMaterials.Cells[row, 5].Value = m.RequestDate.ToString("yyyy-MM-dd");
                row++;
            }
            wsMaterials.Cells[wsMaterials.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();
        }
    }
}
