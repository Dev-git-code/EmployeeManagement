using EmployeeManagement.Models.EmployeeManagement;
using EmployeeManagement.Models.RoleManagement;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Models.Users
{
    public class SeedUsers
    {
        public static async Task Seed(UserManager<IdentityUser> userManager, IEmployeeRepository employeeRepository)
        {
            var users = new List<(string Email, string Password, string Name, Department Department, Roles Role)>
            {
                ("raj.kumar@example.com", "Test@1234", "Raj Kumar", Department.IT, Roles.Employee),
                ("amit.verma@example.com", "Test@1234", "Amit Verma", Department.Sales, Roles.Employee),
                ("vikas.patel@example.com", "Test@1234", "Vikas Patel", Department.IT, Roles.Employee),
                ("rohit.mishra@example.com", "Test@1234", "Rohit Mishra", Department.Marketing, Roles.Employee),
                ("arjun.rana@example.com", "Test@1234", "Arjun Rana", Department.Sales, Roles.Employee),
                ("rahul.sharma@example.com", "Test@1234", "Rahul Sharma", Department.HR, Roles.Employee),
                ("anil.kumar@example.com", "Test@1234", "Anil Kumar", Department.Finance, Roles.Employee),
                ("deepak.singh@example.com", "Test@1234", "Deepak Singh", Department.IT, Roles.Employee),
                ("suresh.nair@example.com", "Test@1234", "Suresh Nair", Department.Finance, Roles.Employee),
                ("vijay.menon@example.com", "Test@1234", "Vijay Menon", Department.Marketing, Roles.Employee)
            };

            foreach (var userInfo in users)
            {
                if (await userManager.FindByEmailAsync(userInfo.Email) == null)
                {
                    var user = new IdentityUser()
                    {
                        UserName = userInfo.Email,
                        Email = userInfo.Email
                    };

                    var employee = new Employee()
                    {
                        Name = userInfo.Name,
                        Email = userInfo.Email,
                        Department = userInfo.Department,
                        Role = userInfo.Role
                    };

                    var result = await userManager.CreateAsync(user, userInfo.Password);
                    if (result.Succeeded)
                    {
                        employeeRepository.Add(employee);
                    }

                    await userManager.AddToRoleAsync(user, userInfo.Role.ToString());
                }
            }
        }
    }
}

