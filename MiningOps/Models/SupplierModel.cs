using System;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class SupplierModel
    {
        // Supplier basic info
        [Required(ErrorMessage = "Supplier name is required")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Supplier name must be between 2 and 255 characters")]
        public string? SupplierName { get; set; }

        // Contract details
        [MaxLength(1000, ErrorMessage = "Contract terms cannot exceed 1000 characters")]
        [Required(ErrorMessage = "Contract terms are required")]
        public string? ContractTerms { get; set; }

        [MaxLength(255, ErrorMessage = "Payment terms cannot exceed 255 characters")]
        [Required(ErrorMessage = "Payment terms are required")]
        public string? PaymentTerms { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Contract start date is required")]
        public DateTime? ContractStartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Contract end date is required")]
        [CustomValidation(typeof(SupplierModel), nameof(ValidateEndDate))]
        public DateTime? ContractEndDate { get; set; }

        // Performance metrics
        [Range(0, 100, ErrorMessage = "On-time delivery rate must be between 0 and 100")]
        public decimal OnTimeDeliveryRate { get; set; } = 0m;

        [Range(0, 5, ErrorMessage = "Quality rating must be between 0 and 5")]
        public int QualityRating { get; set; } = 0;

        [Range(0, 100, ErrorMessage = "Compliance score must be between 0 and 100")]
        public decimal ComplianceScore { get; set; } = 0m;

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Report date is required")]
        public DateTime ReportDate { get; set; } = DateTime.UtcNow;

        // ------------------- Custom Validation -------------------
        public static ValidationResult ValidateEndDate(DateTime? endDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as SupplierModel;
            if (instance.ContractStartDate != null && endDate != null)
            {
                if (endDate < instance.ContractStartDate)
                    return new ValidationResult("Contract end date cannot be before the start date");
            }
            return ValidationResult.Success;
        }
    }
}
