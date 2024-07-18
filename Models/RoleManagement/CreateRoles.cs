using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Models.RoleManagement
{
    public class CreateRoles
    {
        public static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "Employee" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
