﻿using EmployeeManagement.Models.EmployeeManagement;
using EmployeeManagement.Models.RoleManagement;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(IEmployeeRepository employeeRepository,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this._employeeRepository = employeeRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        public ViewResult List()
        {
            var employeeListModel = _employeeRepository.GetAllEmployees();
            return View(employeeListModel);
        }

        public ViewResult Details(int? id)
        {

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);

        }

        [Authorize(Roles="Admin")]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Create(CreateViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = createViewModel.employee.Email,
                    Email = createViewModel.employee.Email
                };

                var result = await _userManager.CreateAsync(user, createViewModel.Password);

                if (result.Succeeded)
                {
                    Employee newEmployee = _employeeRepository.Add(createViewModel.employee);
                    if (createViewModel.employee.Role == Roles.Admin)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Employee");
                    }
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("details", "home", new { id = newEmployee.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(createViewModel);
                

        }

        public ViewResult Edit(int id)
        {
            Employee employeeFromDb = _employeeRepository.GetEmployee(id);
            return View(employeeFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee employeeFromDb = _employeeRepository.GetEmployee(employee.Id);
                employeeFromDb.Name = employee.Name;
                employeeFromDb.Email = employee.Email;
                employeeFromDb.Department = employee.Department;
 
                Employee updatedEmployee = _employeeRepository.Update(employeeFromDb);
                return RedirectToAction("Details",new { id = employeeFromDb.Id });
            }
            return View(employee);
        }

        [Authorize(Roles="Admin")]
        public ViewResult Delete(int id)
        {
            Employee employeeFromDb = _employeeRepository.GetEmployee(id);
            return View(employeeFromDb);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Employee employee)
        {
            Employee employeeFromDb = _employeeRepository.GetEmployee(employee.Id);
            var employeeEmail = employeeFromDb.Email;
            var user = await _userManager.FindByEmailAsync(employeeEmail);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            if (employeeFromDb != null )
            {
                Employee deletedEmployee = _employeeRepository.Delete(employee.Id);    
            }
            return RedirectToAction("List");
           
            
        }
    }
}
