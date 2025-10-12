using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiningOps.Entity
{
    public class Supplier 
    {

        [Key]
        public int SupplierId { get; set; }

        // Foreign Key to RegisterMining (Account/User)
        [Required(ErrorMessage ="Account is required")]
        public int AccId { get; set; }

        [ForeignKey(nameof(AccId))]
        public RegisterMining RegisterMining { get; set; }

        [Required(ErrorMessage ="Company name is required"), MaxLength(200)]
        public string CompanyName { get; set; }    // e.g., Mining Supplies Inc.

        [Required(ErrorMessage ="Contact person is required "), MaxLength(100)]
        public string ContactPerson { get; set; }  // e.g., Yolanda Doe

        [MaxLength(300)]
        public string Address { get; set; }        // e.g., 123 Mining St, City, Country

        // Permissions
        public bool CanViewOrders { get; set; } = true;
        public bool CanManageInventory { get; set; } = false;

        // Navigation

        public ICollection<PurchaseOrder>? PurchaseOrders { get; set; }
        public ICollection<SupplierContract> Contracts { get; set; } = new List<SupplierContract>();
        public ICollection<SupplierPerformance> Performances { get; set; } = new List<SupplierPerformance>();
    }
}
