using MiningOps.Entity;
using Microsoft.EntityFrameworkCore;

namespace MiningOps.Services
{
    public class DashboardAnalyticsService
    {
        private readonly AppDbContext _context;

        public DashboardAnalyticsService(AppDbContext context)
        {
            _context = context;
        }

        // Admin Dashboard Stats
        public async Task<object> GetAdminDashboardAsync()
        {
            var totalSuppliers = await _context.SupplierProfiles.CountAsync();
            var totalOrders = await _context.PurchaseOrdersDb.CountAsync();
            var totalInvoices = await _context.InvoicesDb.CountAsync();
            var totalPayments = await _context.PaymentsDb.SumAsync(p => p.Amount);
            var totalInventoryItems = await _context.InventoryDb.CountAsync();

            var overdueInvoices = await _context.InvoicesDb
                .Where(i => i.Status == Entity.roleFolder.InvoiceStatus.Overdue)
                .CountAsync();

            return new
            {
                totalSuppliers,
                totalOrders,
                totalInvoices,
                totalPayments,
                totalInventoryItems,
                overdueInvoices
            };
        }

        // Supplier Dashboard Stats
        public async Task<object> GetSupplierDashboardAsync(int supplierId)
        {
            var totalOrders = await _context.PurchaseOrdersDb
                .Where(o => o.SupplierId == supplierId)
                .CountAsync();

            var totalInvoices = await _context.InvoicesDb
                .Where(i => i.Order.SupplierId == supplierId)
                .CountAsync();

            var paidInvoices = await _context.InvoicesDb
                .Where(i => i.Order.SupplierId == supplierId && i.Status == Entity.roleFolder.InvoiceStatus.Paid)
                .CountAsync();

            var pendingPayments = await _context.PaymentsDb
                .Where(p => p.Invoice.Order.SupplierId == supplierId)
                .SumAsync(p => p.Amount);

            return new
            {
                totalOrders,
                totalInvoices,
                paidInvoices,
                pendingPayments
            };
        }
    }
}
