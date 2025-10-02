using MiningOps.Entity.roleFolder;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Entity
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public PurchaseOrder Order { get; set; }

        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public decimal Amount { get; set; } = 0m;
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Unpaid;
        public string? InvoiceReference { get; set; }

        // Optionally, supplier uploaded file reference
        public string? InvoiceFilePath { get; set; }
    }
}
  