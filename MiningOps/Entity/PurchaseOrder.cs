using MiningOps.Entity.roleFolder;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Entity
{
    public class PurchaseOrder
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; }

        [Required]
        public int RequestedBy { get; set; } // RegisterMining.AccId
        [ForeignKey(nameof(RequestedBy))]
        public RegisterMining Requester { get; set; }

        public string? Currency { get; set; } = "ZAR";

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpectedDeliveryDate { get; set; }

        public decimal TotalAmount { get; set; } = 0m;

        // Navigation
        public ICollection<OrderItem>? Items { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
    }
}

