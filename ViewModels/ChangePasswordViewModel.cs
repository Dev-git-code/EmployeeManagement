using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string Id {  get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string? OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string? ConfirmPassword { get; set; }
    }
}
