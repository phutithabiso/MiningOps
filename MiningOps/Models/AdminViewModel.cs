using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class AdminViewModel 
    {
        public int AccId { get; set; }
        public string? Department { get; set; }       // e.g., Procurement, IT
        public bool CanManageUsers { get; set; } = true;
        public bool CanApproveRequests { get; set; } = true;
    }
}
