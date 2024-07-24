using EmployeeManagement.Models.EmployeeManagement;
using EmployeeManagement.Models.RoleManagement;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EmployeeManagement.Models.Users
{
    public class SuperAdmin
    {
        public static async Task CreateSuperAdmin(UserManager<IdentityUser> userManager,
            IEmployeeRepository employeeRepository)
        {
            string email = "admin@admin.com";
            string password = "Test@1234";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser()
                {
                    UserName = email,
                    Email = email
                };

                //var result = await userManager.CreateAsync(user, password);

                    // Create and add Employee with the foreign key reference
                    var employee = new Employee()
                    {
                        Name = "Super Admin",
                        Email = email,
                        Department = Department.None,
                        Role = Roles.Admin,
                        IdentityUserId = user.Id  // Set the foreign key
                    };

                    await employeeRepository.Add(employee, password);

                    // Assign the user to the Admin role
                    await userManager.AddToRoleAsync(user, "Admin");
                
            }
        }
    }
}
