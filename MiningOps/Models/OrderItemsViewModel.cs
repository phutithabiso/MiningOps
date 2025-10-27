using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MiningOps.Entity;

namespace MiningOps.Models
{
    public class OrderItemsViewModel
    {
        [Key]
        //public int OrderItemId { get; set; }

        [Required(ErrorMessage = "Item Name is required")]
        [MaxLength(200)]
        public string? ItemName { get; set; } = string.Empty;

        [Required(ErrorMessage = " Quantity is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit Price is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;

        // Foreign Key to PurchaseOrder
        [Required(ErrorMessage = "Purchase Order is required")]
        public int PurchaseOrderId { get; set; }

        [ForeignKey(nameof(PurchaseOrderId))]
        public PurchaseOrder? PurchaseOrder { get; set; }

    }
}