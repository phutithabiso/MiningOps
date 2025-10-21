using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class SupplierPerformanceViewModel
    {
        [Key]
        public int PerformanceId { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        [Range(0, 100, ErrorMessage = "On-time delivery rate must be between 0 and 100")]
        [Display(Name = "On-Time Delivery Rate (%)")]
        public decimal OnTimeDeliveryRate { get; set; }

        [Range(0, 5, ErrorMessage = "Quality rating must be between 0 and 5")]
        [Display(Name = "Quality Rating")]
        public int QualityRating { get; set; }

        [Range(0, 100, ErrorMessage = "Compliance score must be between 0 and 100")]
        [Display(Name = "Compliance Score (%)")]
        public decimal ComplianceScore { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Report Date")]
        public DateTime ReportDate { get; set; } = DateTime.UtcNow;
        public IEnumerable<SelectListItem>? Suppliers { get; set; }
    }
}
