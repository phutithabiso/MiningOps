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




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisterMining>()
    .Property(u => u.PasswordHash)
    .HasMaxLength(500); // adjust if needed

            modelBuilder.Entity<RegisterMining>()
                .Property(u => u.Salt)
                .HasMaxLength(500); // usually much shorter, but safe
            modelBuilder.Entity<InventoryItem>()
       .Property(i => i.UnitCost)
       .HasColumnType("decimal(18,2)");



            base.OnModelCreating(modelBuilder);
        }
    }
}
