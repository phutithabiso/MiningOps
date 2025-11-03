using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "New password must be at least 8 characters long")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm your new password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmNewPassword { get; set; }
    }
}
