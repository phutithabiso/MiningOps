using MiningOps.Entity;
using MiningOps.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MiningOps.Models
{
    public class SupplierDashboardViewModel
    {
        [Key]
        // Supplier info
        public int SupId { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }

        // Contracts & performance
        public IEnumerable<SupplierContract> SupplierContracts { get; set; } = new List<SupplierContract>();
        public IEnumerable<SupplierPerformance> SupplierPerformance { get; set; } = new List<SupplierPerformance>();

        // Contract stats
        public int TotalContractCount => SupplierContracts.Count();
        public int ActiveContractCount { get; set; }
        // public int ActiveContractCount => SupplierContracts.Count(c => c.EndDate >= DateTime.Now);
        public int ExpiredContractCount => SupplierContracts.Count(c => c.EndDate < DateTime.Now);
        // public int ExpiringContractCount => SupplierContracts.Count(c => c.EndDate < DateTime.Now.AddDays(30) && c.EndDate >= DateTime.Now);
        public int ExpiringContractCount { get; set; }
        // Performance stats
        public int TotalPerformanceRecords => SupplierPerformance.Count();
        //public decimal AverageDeliveryRate => SupplierPerformance.Any() ? SupplierPerformance.Average(p => p.OnTimeDeliveryRate) : 0;
        //public decimal AverageQualityRating => SupplierPerformance.Any() ? SupplierPerformance.Average(p => p.QualityRating) : 0;

        public decimal AverageQualityRating { get; set; }
        public decimal AverageDeliveryRate { get; set; }
        public decimal AverageComplianceScore { get; set; }

        //  public decimal AverageComplianceScore => SupplierPerformance.Any() ? SupplierPerformance.Average(p => p.ComplianceScore) : 0;

    }
}
