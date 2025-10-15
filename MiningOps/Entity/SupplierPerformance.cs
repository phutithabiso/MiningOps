using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace MiningOps.Entity
{
    public class SupplierPerformance
    {
        [Key]
        public int PerformanceId { get; set; }

        [Required(ErrorMessage ="Supplier name is required") ]
        public int SupplierId { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; }

        public decimal OnTimeDeliveryRate { get; set; } = 0m; // 0..100
        public int QualityRating { get; set; } = 0; // 0..5
        public decimal ComplianceScore { get; set; } = 0m;
        public DateTime ReportDate { get; set; } = DateTime.UtcNow;

    }
}
