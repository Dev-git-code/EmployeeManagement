using EmployeeManagement.Models.EmployeeManagement;
using EmployeeManagement.Models.RoleManagement;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class ApplicationUser : IdentityUser
{
    // Additional properties for ApplicationUser
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = "";

    [Required]
    public Department? Department { get; set; }

    [Required]
    public Roles? Role { get; set; }

}
