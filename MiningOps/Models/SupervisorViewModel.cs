using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class SupervisorViewModel 
    {
        public int AccId { get; set; }
        public string? Team { get; set; }              // e.g., Maintenance, Operations
        [MaxLength(200)]
        public string? MineLocation { get; set; }

        [MaxLength(20)]
        public string? Shift { get; set; } // Day/Night
        public bool CanViewReports { get; set; } = true;
        public bool CanManageTasks { get; set; } = true;
    }
}
