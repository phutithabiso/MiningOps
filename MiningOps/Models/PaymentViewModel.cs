using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class PaymentViewModel
    {
        [Key]
      

        [Required(ErrorMessage = "Invoice is required")]
        [Display(Name = "Invoice")]
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [Display(Name = "Payment Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Payment Date")]
        public DateTime PaidDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Approved By (Admin)")]
        public int? ApprovedById { get; set; }  // Optional admin approval

        [MaxLength(100)]
        [Display(Name = "Payment Reference")]
        public string? PaymentReference { get; set; }
    }
}
