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

        [Authorize(Roles="Admin")]
        public async Task<ViewResult> List()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                var Currentuser = await _userManager.FindByIdAsync(id);
            }
            if (currentUser.Id == id || currentUser.Role == Roles.Admin)
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    Response.StatusCode = 404;
                    return View("EmployeeNotFound", id);
                }

                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
                {
                    ApplicationUser = user,
                    PageTitle = "Employee Details"
                };
                return View(homeDetailsViewModel);
            }
            return RedirectToAction("AccessDenied", "Account");
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
                    // Assign the role to the newly created user
                    if (createViewModel.Role == Roles.Admin)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Employee");
                    }

                    // Optional: Sign in the new user
                    // await _signInManager.SignInAsync(user, isPersistent: false);

                    TempData["success"] = "The user has been created successfully";
                    return RedirectToAction("Details", "Home", new { id = user.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            TempData["error"] = "The user could not be created";
            return View(createViewModel);
        }

        public async Task<ViewResult> Edit(string id)
        {
            var employeeFromDb = await _userRepository.GetUserByIdAsync(id);
            return View(employeeFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
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

                    // Update user properties
                    userFromDb.Email = user.Email;
                    userFromDb.UserName = user.Email; // Username usually matches email
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

            TempData["error"] = "The user details could not be updated";
            return View(user);
        }



        [Authorize(Roles="Admin")]
        public async Task<ViewResult> Delete(string id)
        {
            var employeeFromDb = await _userRepository.GetUserByIdAsync(id);
            return View(employeeFromDb);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            if (user == null)
            {
                TempData["error"] = "User not found";
                return RedirectToAction("List");
            }

            // Fetch the user from the database to ensure it exists
            var userFromDb = await _userManager.FindByIdAsync(user.Id);
            if (userFromDb == null)
            {
                TempData["error"] = "User not found";
                return RedirectToAction("List");
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(userFromDb);

            if (result.Succeeded)
            {
                TempData["success"] = "The user has been deleted successfully";
                return RedirectToAction("List");
            }

            // If deletion fails, add errors to ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            TempData["error"] = "The user could not be deleted";
            return View("Error");
        }


    }
}
