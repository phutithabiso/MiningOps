using MiningOps.Entity.roleFolder;
using MiningOps.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class PurchaseOrderViewModel
    {
        [Key]
        // public int OrderId { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Requester is required")]
        public int RequestedBy { get; set; }

        [Display(Name = "Currency")]
        public string Currency { get; set; } = "ZAR";

        [Display(Name = "Order Status")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Display(Name = "Expected Delivery Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [Display(Name = "Total Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Material Request")]
        public int? MaterialRequestId { get; set; }
    }
}
