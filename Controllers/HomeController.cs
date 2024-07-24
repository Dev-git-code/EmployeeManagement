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
                var newEmployee = await _employeeRepository.Add(createViewModel.employee, createViewModel.Password);
                TempData["success"] = "The Employee has been created successfully";
                return RedirectToAction("details", "home", new { id = newEmployee.Id });
                

               /* foreach (var error in newEmployee.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }*/
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
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {               
                Employee updatedEmployee = await _employeeRepository.Update(employee);
                TempData["success"] = "The Employee details has been updated successfully";
                return RedirectToAction("Details",new { id = updatedEmployee.Id });
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
