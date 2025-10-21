using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiningOps.Entity
{
    public class Supervisor
    {

        [Key]
        public int SupervisorId { get; set; }

        // Foreign Key to RegisterMining (Account/User)
        [Required(ErrorMessage = "Account is required")]
        public int AccId { get; set; }

        [ForeignKey(nameof(AccId))]
        public RegisterMining? RegisterMining { get; set; }
        public string? Team { get; set; }              // e.g., Maintenance, Operations
        [MaxLength(200)]
        public string? MineLocation { get; set; }


        [MaxLength(20)]
        public string? Shift { get; set; } // Day/Night
        public bool CanViewReports { get; set; } = true;
        public bool CanManageTasks { get; set; } = true;

       // public RegisterMining RegisterMining { get; set; }
    }
}
