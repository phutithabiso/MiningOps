using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class SupplierViewModel
    {
        // Additional properties or methods specific to Supplier can be added here
        
        public int AccId { get; set; }
        public string? CompanyName { get; set; }    // e.g., Mining Supplies Inc.
        public string? ContactPerson { get; set; }  // e.g., John Doe
        public string? Address { get; set; }        // e.g., 123 Mining St, City, Country
        public bool CanViewOrders { get; set; } = true;
        public bool CanManageInventory { get; set; } = false;
    }
}
