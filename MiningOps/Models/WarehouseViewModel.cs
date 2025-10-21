using MiningOps.Entity;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class WarehouseViewModel
    {
        [Key]
 

        [Required(ErrorMessage = "Warehouse name is required"), MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(300)]
        public string? Location { get; set; }

        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<InventoryItem>? InventoryItems { get; set; }
    }
}
