using EmployeeManagement.Models.EmployeeManagement;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class CreateViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public Employee employee { get; set; }
    }
}
