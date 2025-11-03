using MiningOps.Entity.roleFolder;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class InvoiceViewModel
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Purchase Order is required")]
        [Display(Name = "Purchase Order")]
        public int OrderId { get; set; }

        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

   
       // [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        //[Display(Name = "Invoice Amount")]
        public decimal Amount { get; set; } = 0m;

        [Display(Name = "Status")]
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Unpaid;

        [MaxLength(100)]
        [Display(Name = "Invoice Reference")]
        public string? InvoiceReference { get; set; }

        [Display(Name = "Invoice File Path")]
        public string? InvoiceFilePath { get; set; }
    }
}
