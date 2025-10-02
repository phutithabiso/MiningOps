using MiningOps.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class InvetoryViewModel
    {
        [Key]
        public int InventoryId { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [MaxLength(150)]
        public string ItemName { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or more")]
        public int Quantity { get; set; } = 0;

        [Range(0, int.MaxValue, ErrorMessage = "Reorder level must be 0 or more")]
        public int ReorderLevel { get; set; } = 0;

        [Required(ErrorMessage = "Unit cost is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Unit cost must be positive")]
        public decimal UnitCost { get; set; } = 0m;

        // Foreign Key → link to Warehouse
        [Required(ErrorMessage = "Warehouse is required")]
        public int WarehouseId { get; set; }

        [ForeignKey(nameof(WarehouseId))]
        public Warehouse Warehouse { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? Unit { get; set; } // e.g., "Kg", "Ton", "Litre"
    }
}
