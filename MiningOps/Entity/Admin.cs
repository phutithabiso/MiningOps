using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MiningOps.Entity
{
    public class Admin
    {
        // Additional properties or methods specific to Admin can be added here
        // [Key, ForeignKey("RegisterMining")]
        // public int AccId { get; set; }
        [Key]
        public int AdminId { get; set; }

        // Foreign Key to RegisterMining (Account/User)
        [Required(ErrorMessage = "Account is required")]
        public int AccId { get; set; }

        [ForeignKey(nameof(AccId))]
        public RegisterMining? RegisterMining { get; set; }
        public string Department { get; set; }       // e.g., Procurement, IT
        public bool CanManageUsers { get; set; } = true;
        public bool CanApproveRequests { get; set; } = true;
        //public RegisterMining RegisterMining { get; set; }
    }
}
