using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MiningOps.Entity
{
    public class MaterialRequest
    {
        [Key]
        public int MaterialRequestId { get; set; }

        [Required(ErrorMessage="Item Name is Required")]
        [MaxLength(200)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // e.g. Pending, Approved, Rejected

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<PurchaseOrder>? PurchaseOrders { get; set; }
    }
}
