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

        public async Task<IActionResult> Details(int? id)
        {
            var employeeFromDb = await _employeeRepository.GetEmployeeByEmailAsync(User.Identity.Name);
            if(employeeFromDb.Id == id || employeeFromDb.Role == Roles.Admin)
            {
                Employee employee = _employeeRepository.GetEmployee(id ?? 1);
                if (employee == null)
                {
                    Response.StatusCode = 404;
                    return View("EmployeeNotFound", id);
                }
                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
                {
                    Employee = employee,
                    PageTitle = "Employee Details"
                };
                return View(homeDetailsViewModel);
            }
            return RedirectToAction("AccessDenied","Account");
           

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
                    TempData["success"] = "The Employee has been created successfully";
                    return RedirectToAction("details", "home", new { id = newEmployee.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            TempData["error"] = "The Employee could not be created";
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
                TempData["success"] = "The Employee details has been updated successfully";
                return RedirectToAction("Details",new { id = employeeFromDb.Id });
            }
            TempData["error"] = "The Employee details could not be updated";
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
                if (employeeFromDb != null)
                {
                    Employee deletedEmployee = _employeeRepository.Delete(employee.Id);               
                }
                TempData["success"] = "The Employee details has been deleted successfully";
                return RedirectToAction("List");
            }

            TempData["error"] = "The Employee details could not be deleted";
            return View(employeeFromDb);
            
        }
    }
}
