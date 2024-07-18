using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Models.RoleManagement;

namespace EmployeeManagement.Models.EmployeeManagement
{
    public class Employee
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; } = "";
        [Display(Name = "Office Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid email format")]
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public Department? Department { get; set; }

        [Required]
        public Roles? Role { get; set; }
    }
}
