using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Entity
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required(ErrorMessage ="Item Name is required")]
        [MaxLength(200)]
        public string ItemName { get; set; } = string.Empty;

        [Required(ErrorMessage =" Quantity is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage ="Unit Price is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;

        // Foreign Key to PurchaseOrder
        public int PurchaseOrderId { get; set; }
        [ForeignKey("PurchaseOrderId")]
        public PurchaseOrder? PurchaseOrder { get; set; }
    }
}
