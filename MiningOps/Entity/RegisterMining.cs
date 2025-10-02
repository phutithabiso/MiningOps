using System.ComponentModel.DataAnnotations;
using System;

namespace MiningOps.Entity
{
    public class RegisterMining
    {
        [Key]
        public int AccId { get; set; }
        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [MaxLength(15, ErrorMessage = "Phone Number cannot exceed 15 characters")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(500, ErrorMessage = "Password cannot exceed 500 characters")]

        public string PasswordHash { get; set; }   // Store hashed password

        [Required]

        [MaxLength(200)]
        public string Salt { get; set; }           // Unique per user

        [Required]
        public UserRole Role { get; set; }           // Admin, Supervisor, Supplier

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        // Navigation (1-to-1 relationship)
        public Admin? AdminProfile { get; set; }
        public Supervisor? SupervisorProfile { get; set; }
        public Supplier? SupplierProfile { get; set; }

        // 1:n
        //public ICollection<MaterialRequest>? MaterialRequests { get; set; }
        //public ICollection<PurchaseOrder>? CreatedPurchaseOrders { get; set; }
        //public ICollection<Invoice>? ApprovedInvoices { get; set; }
        //public ICollection<AuditLog>? AuditLogs { get; set; }

    }
}
