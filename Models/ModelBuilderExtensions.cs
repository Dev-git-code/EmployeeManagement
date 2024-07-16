﻿
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Vipin", Department = Department.Customer_Support, Email = "vipin@accops.com" },
                new Employee { Id = 2, Name = "Rajiv", Department = Department.Sales, Email = "rajiv@accops.com" },
                new Employee { Id = 3, Name = "Dev", Department = Department.IT_Infra, Email = "dev@accops.com" },
                new Employee { Id = 4, Name = "Kartik", Department = Department.Sales, Email = "kartik@accops.com" },
                new Employee { Id = 5, Name = "Samarth", Department = Department.HR, Email = "samarth@accops.com" },
                new Employee { Id = 6, Name = "Akash", Department = Department.Engineering, Email = "akash@accops.com" },
                new Employee { Id = 7, Name = "Arjun", Department = Department.IT_Infra, Email = "arjun@accops.com" },
                new Employee { Id = 8, Name = "Risabh Rai", Department = Department.Customer_Support, Email = "risabh.rai@accops.com" }
             );
        }
    }
}