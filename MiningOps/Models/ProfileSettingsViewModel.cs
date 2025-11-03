using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class ProfileSettingsViewModel
    {
        public int AccId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
    }
}
