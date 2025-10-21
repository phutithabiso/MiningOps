using MiningOps.Entity;
using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class RegisterViewModel
    {
        [Key]
        public int MaterialRequestId { get; set; }
        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(100)]
        public string Email { get; set;  }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8)]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}

