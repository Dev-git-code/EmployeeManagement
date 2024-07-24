using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Models.EmployeeManagement
{
    public class ApplicationUser:IdentityUser
    {
        // Navigation property for the related Employees
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
