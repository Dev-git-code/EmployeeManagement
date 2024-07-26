using EmployeeManagement.Models.EmployeeManagement;
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
        private readonly IApplicationUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(IApplicationUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this._userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<ViewResult> List()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while retrieving the user list.";               
                return View("Error"); 
            }
        }


        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if (currentUser == null)
                {
                    currentUser = await _userManager.FindByIdAsync(id);
                }

                if (currentUser != null && (currentUser.Id == id || currentUser.Role == Roles.Admin))
                {
                    var user = await _userRepository.GetUserByIdAsync(id);
                    if (user == null)
                    {
                        Response.StatusCode = 404;
                        return View("EmployeeNotFound", id);
                    }

                    var homeDetailsViewModel = new HomeDetailsViewModel
                    {
                        ApplicationUser = user,
                        PageTitle = "Employee Details"
                    };
                    return View(homeDetailsViewModel);
                }

                return RedirectToAction("AccessDenied", "Account");
            }
            catch
            {
                TempData["error"] = "An error occurred while retrieving the employee details.";
                return RedirectToAction("Index", "Home");
            }
        }



        [Authorize(Roles="Admin")]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new ApplicationUser
                    {
                        UserName = createViewModel.Email,
                        Email = createViewModel.Email,
                        Name = createViewModel.Name,
                        Department = createViewModel.Department,
                        Role = createViewModel.Role
                    };

                    var result = await _userManager.CreateAsync(user, createViewModel.Password);

                    if (result.Succeeded)
                    {
                        var role = createViewModel.Role == Roles.Admin ? "Admin" : "Employee";
                        await _userManager.AddToRoleAsync(user, role);

                        TempData["success"] = "The user has been created successfully";
                        return RedirectToAction("Details", "Home", new { id = user.Id });
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch
                {
                    TempData["error"] = "An error occurred while creating the user.";
                    return View(createViewModel);
                }
            }

            TempData["error"] = "The user could not be created.";
            return View(createViewModel);
        }


        public async Task<ViewResult> Edit(string id)
        {
            try
            {
                var employeeFromDb = await _userRepository.GetUserByIdAsync(id);
                if (employeeFromDb == null)
                {
                    TempData["error"] = "Employee not found.";
                    return View("Error"); 
                }
                return View(employeeFromDb);
            }
            catch
            {
                TempData["error"] = "An error occurred while retrieving the employee details.";
                return View("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            try
            {
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if (currentUser.Id == user.Id || currentUser.Role == Roles.Admin)
                {
                    if (ModelState.IsValid)
                    {
                        var userFromDb = await _userManager.FindByIdAsync(user.Id);
                        if (userFromDb == null)
                        {
                            Response.StatusCode = 404;
                            return View("UserNotFound", user.Id);
                        }

                        userFromDb.Email = user.Email;
                        userFromDb.UserName = user.Email;
                        userFromDb.Name = user.Name;
                        userFromDb.Department = user.Department;
                        userFromDb.Role = user.Role;

                        var result = await _userManager.UpdateAsync(userFromDb);

                        if (result.Succeeded)
                        {
                            TempData["success"] = "The user details have been updated successfully";
                            return RedirectToAction("Details", new { id = userFromDb.Id });
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }
            catch
            {
                TempData["error"] = "An error occurred while updating the user details.";
                return View(user);
            }

            TempData["error"] = "The user details could not be updated";
            return View(user);
        }



        [Authorize(Roles = "Admin")]
        public async Task<ViewResult> Delete(string id)
        {
            try
            {
                var employeeFromDb = await _userRepository.GetUserByIdAsync(id);
                if (employeeFromDb == null)
                {
                    TempData["error"] = "Employee not found.";
                    return View("Error"); 
                }
                return View(employeeFromDb);
            }
            catch
            {
                TempData["error"] = "An error occurred while retrieving the employee details.";
                return View("Error"); 
            }
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            try
            {
                if (user == null)
                {
                    TempData["error"] = "User not found";
                    return RedirectToAction("List");
                }

                var userFromDb = await _userManager.FindByIdAsync(user.Id);
                if (userFromDb == null)
                {
                    TempData["error"] = "User not found";
                    return RedirectToAction("List");
                }

                var result = await _userManager.DeleteAsync(userFromDb);

                if (result.Succeeded)
                {
                    TempData["success"] = "The user has been deleted successfully";
                    return RedirectToAction("List");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                TempData["error"] = "The user could not be deleted";
                return View("Error");
            }
            catch
            {
                TempData["error"] = "An error occurred while deleting the user.";
                return View("Error");
            }
        }



    }
}
