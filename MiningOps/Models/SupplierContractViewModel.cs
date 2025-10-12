using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class SupplierContractViewModel
    {
       // public int ContractId { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        [StringLength(1000, ErrorMessage = "Contract terms cannot exceed 1000 characters")]
        [Display(Name = "Contract Terms")]
        public string? ContractTerms { get; set; }

        [StringLength(255, ErrorMessage = "Payment terms cannot exceed 255 characters")]
        [Display(Name = "Payment Terms")]
        public string? PaymentTerms { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
    }
}
