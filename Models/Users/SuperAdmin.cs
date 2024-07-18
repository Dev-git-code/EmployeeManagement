using EmployeeManagement.Models.EmployeeManagement;
using EmployeeManagement.Models.RoleManagement;
using Microsoft.AspNetCore.Identity;

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

                var employee = new Employee()
                {
                    Name = "Super Admin",
                    Email = email,
                    Department = Department.None,
                    Role = Roles.Admin
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    employeeRepository.Add(employee);
                }

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
