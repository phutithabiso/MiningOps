using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Entity
{
    public class InventoryItem
    {
        [Key]
        public int InventoryId { get; set; }

        [Required(ErrorMessage ="Item Name is Required"), MaxLength(150)]
        public string ItemName { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; } = 0;

        public int ReorderLevel { get; set; } = 0;

        public decimal UnitCost { get; set; } = 0m;

        public int WarehouseId { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        public Warehouse Warehouse { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Optional: track unit of measure
        [MaxLength(50)]
        public string? Unit { get; set; }
    }
}
