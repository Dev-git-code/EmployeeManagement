using EmployeeManagement.Models.EmployeeManagement;
using EmployeeManagement.Models.RoleManagement;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EmployeeManagement.Models.Users
{
    public class SuperAdmin
    {
        public static async Task CreateSuperAdmin(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string email = "admin@admin.com";
            string password = "Test@1234";

            // Ensure the Admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = email,
                    Email = email,
                    Name = "Super Admin",
                    Department = Department.None,  // Assuming there's a Department.None for this purpose
                    Role = Roles.Admin
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
