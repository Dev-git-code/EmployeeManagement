﻿using EmployeeManagement.Models.EmployeeManagement;
using EmployeeManagement.Models.RoleManagement;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class CreateViewModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Department Department { get; set; }

        [Required]
        public Roles Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
