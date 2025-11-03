using Microsoft.EntityFrameworkCore;
using MiningOps.Models;

namespace MiningOps.Entity
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<RegisterMining> RegisterMiningDb { get; set; }
        public DbSet<Admin> AdminProfiles { get; set; }
        public DbSet<Supervisor> SupervisorProfiles { get; set; }
        public DbSet<Supplier> SupplierProfiles { get; set; }
        public DbSet<Warehouse> WarehousesDb { get; set; }
        public DbSet<InventoryItem> InventoryDb { get; set; }
        public DbSet<SupplierContract> SupplierContractDb { get; set; }
        public DbSet<SupplierPerformance> SupplierPerformanceDb { get; set; }
        public DbSet<OrderItem> OrderItemsDb { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrdersDb { get; set; }
        public DbSet<MaterialRequest> MaterialRequestsDb { get; set; }
        public DbSet<Payment> PaymentsDb { get; set; }
        public  DbSet<Invoice> InvoicesDb { get; set; }
       






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<RegisterMining>()
    .Property(u => u.PasswordHash)
    .HasMaxLength(500); // adjust if needed

            modelBuilder.Entity<RegisterMining>()
                .Property(u => u.Salt)
                .HasMaxLength(500);
            modelBuilder.Entity<InventoryItem>()
       .Property(i => i.UnitCost)
       .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PurchaseOrder>()
       .HasOne(p => p.Supplier)
       .WithMany(s => s.PurchaseOrders)
       .HasForeignKey(p => p.SupplierId)
       .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<PurchaseOrder>()
          .HasOne(po => po.MaterialRequest)
          .WithMany(mr => mr.PurchaseOrders)
          .HasForeignKey(po => po.MaterialRequestId)
          .OnDelete(DeleteBehavior.Cascade);
         
            modelBuilder.Entity<PurchaseOrder>()
                .Property(po => po.TotalAmount)
                  .HasPrecision(18, 2);

            modelBuilder.Entity<SupplierPerformance>()
      .Property(sp => sp.OnTimeDeliveryRate)
      .HasPrecision(18, 2); // 18 total digits, 2 decimal places

            modelBuilder.Entity<SupplierPerformance>()
                .Property(sp => sp.ComplianceScore)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
      .Property(p => p.Amount)
      .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.UnitPrice)
                .HasColumnType("decimal(18,2)");
           

            modelBuilder.Entity<SupplierContract>()
                .Property(c => c.ContractValue)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
               .HasOne(o => o.PurchaseOrder)
               .WithMany(p => p.Items)
               .HasForeignKey(o => o.PurchaseOrderId)
               .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<MaterialRequest>()
            //    .HasOne(m => m.Supplier)
            //    .WithMany(s => s.MaterialRequests)
            //    .HasForeignKey(m => m.SupplierId)
            //    .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
      

      
   
    }
}
