using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiningOps.Entity
{
    public class SupplierContract
    {
        [Key]
        public int ContractId { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        public int SupplierId { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; }

        [MaxLength(1000)]
        public string? ContractTerms { get; set; }   // Terms of the contract

        [MaxLength(255)]
        public string? PaymentTerms { get; set; }    // e.g., Net 30 days

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
