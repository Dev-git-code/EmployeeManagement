using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

using EmployeeManagement.Models.RoleManagement;

namespace EmployeeManagement.Models.EmployeeManagement
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Vipin", Department = Department.Finance, Email = "vipin@accops.com", Role = Roles.Employee },
                new Employee { Id = 2, Name = "Rajiv", Department = Department.Sales, Email = "rajiv@accops.com", Role = Roles.Employee },
                new Employee { Id = 3, Name = "Dev", Department = Department.IT, Email = "dev@accops.com", Role = Roles.Employee },
                new Employee { Id = 4, Name = "Kartik", Department = Department.Sales, Email = "kartik@accops.com", Role = Roles.Employee },
                new Employee { Id = 5, Name = "Samarth", Department = Department.HR, Email = "samarth@accops.com", Role = Roles.Employee },
                new Employee { Id = 6, Name = "Akash", Department = Department.Engineering, Email = "akash@accops.com", Role = Roles.Employee },
                new Employee { Id = 7, Name = "Arjun", Department = Department.Sales, Email = "arjun@accops.com", Role = Roles.Employee },
                new Employee { Id = 8, Name = "Risabh Rai", Department = Department.Engineering, Email = "risabh.rai@accops.com", Role = Roles.Employee }
            );
        }
    }
}
