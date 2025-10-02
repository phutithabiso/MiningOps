using System.ComponentModel.DataAnnotations;

namespace MiningOps.Entity
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "Warehouse name is required"), MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string? Location { get; set; }

        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<InventoryItem>? InventoryItems { get; set; }
    }
}
