using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Entity
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int InvoiceId { get; set; }
        [ForeignKey(nameof(InvoiceId))]

        public Invoice? Invoice { get; set; }

        public DateTime PaidDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0m;

        public int? ApprovedById { get; set; } // Admin who approved
        [ForeignKey(nameof(ApprovedById))]
        public RegisterMining? ApprovedBy { get; set; }

        public string? PaymentReference { get; set; }
    }
}
