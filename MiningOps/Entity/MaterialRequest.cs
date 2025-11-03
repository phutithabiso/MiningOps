using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MiningOps.Entity
{
    public class MaterialRequest
    {

        [Key]
        public int MaterialRequestId { get; set; }

        [Required(ErrorMessage = "Item Name is required")]
        [MaxLength(200)]
        public string? ItemName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Status { get; set; } = "Pending";

        [Required]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; } = DateTime.Now;
       

        public ICollection<PurchaseOrder>? PurchaseOrders { get; set; }

    }
}
