using EmployeeManagement.Models.EmployeeManagement;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmployeeRepository _employeeRepository;


        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IEmployeeRepository employeeRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeRepository = employeeRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    Employee employeeFromDb = await _employeeRepository.GetEmployeeByEmailAsync(model.Email);
                    TempData["success"] = "Login Successful";
                    return RedirectToAction("details", "home", new { id = employeeFromDb.Id });
                }
                TempData["error"] = "Login Unsuccessful";
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = registerViewModel.employee.Email,
                    Email = registerViewModel.employee.Email
                };

                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    Employee newEmployee = await _employeeRepository.Add(registerViewModel.employee, registerViewModel.Password);
                    Employee EmployeeFromDb = await _employeeRepository.GetEmployeeByEmailAsync(registerViewModel.employee.Email);  
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["success"] = "The Employee has been registered successfully";
                    return RedirectToAction("details","home", new { id = EmployeeFromDb.Id });
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            TempData["error"] = "The Employee could not be registered";
            return View(registerViewModel);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
