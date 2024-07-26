using EmployeeManagement.Models.EmployeeManagement;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    TempData["success"] = "Login Successful";
                    return RedirectToAction("details", "home", new { id = user.Id });
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
                var user = new ApplicationUser
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    Name = registerViewModel.Name,
                    Department = registerViewModel.Department,
                    Role = registerViewModel.Role
                };

                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["success"] = "The Employee has been registered successfully";
                    return RedirectToAction("details", "home", new { id = user.Id });
                }

                foreach (var error in result.Errors)
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

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            if (User.Identity.Name == currentUser.Email)
            {
                var changePasswordViewModel = new ChangePasswordViewModel
                {
                    Id = currentUser.Id
                };
                return View(changePasswordViewModel);
            }
            return RedirectToAction("AccessDenied", "Account");
        }

        [HttpPost]
        
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(changePasswordViewModel.Id);
                if(user.Email == User.Identity.Name)
                {
                    if (user != null)
                    {
                        IdentityResult result = await _userManager.ChangePasswordAsync(user,
                                                                changePasswordViewModel.OldPassword,
                                                                changePasswordViewModel.NewPassword);
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            TempData["success"] = "The Employee password has been successfully";
                            return RedirectToAction("details", "home", new { id = user.Id });
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                return RedirectToAction("AccessDenied", "Account");
               
            }
            TempData["error"] = "The Employee Password could not be Changed.";
            return View(changePasswordViewModel);
        }

     }
}
