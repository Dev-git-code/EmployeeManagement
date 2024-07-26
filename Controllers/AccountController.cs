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
                try
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
                catch (Exception ex)
                {
                    TempData["error"] = "An error occurred during login.";
                    ModelState.AddModelError(string.Empty, "An error occurred. Please try again later.");
                }
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
                try
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
                catch (Exception ex)
                {
                    TempData["error"] = "An error occurred during registration.";
                    ModelState.AddModelError(string.Empty, "An error occurred. Please try again later.");
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
            try
            {
                var currentUser = await _userManager.FindByIdAsync(id);
                if (currentUser == null)
                {
                    TempData["error"] = "User not found.";
                    return RedirectToAction("Index", "Home");
                }

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
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while loading the change password page.";
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(changePasswordViewModel.Id);
                    if (user == null)
                    {
                        TempData["error"] = "User not found.";
                        return RedirectToAction("Index", "Home");
                    }

                    if (user.Email == User.Identity.Name)
                    {
                        IdentityResult result = await _userManager.ChangePasswordAsync(user,
                                                        changePasswordViewModel.OldPassword,
                                                        changePasswordViewModel.NewPassword);
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            TempData["success"] = "The Employee password has been successfully changed.";
                            return RedirectToAction("details", "home", new { id = user.Id });
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    else
                    {
                        return RedirectToAction("AccessDenied", "Account");
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = "An error occurred while changing the password.";
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                }
            }

            TempData["error"] = "The Employee Password could not be changed.";
            return View(changePasswordViewModel);
        }


    }
}
