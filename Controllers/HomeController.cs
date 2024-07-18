using EmployeeManagement.Models;
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

        [Authorize]
        public ViewResult List()
        {
            var employeeListModel = _employeeRepository.GetAllEmployees();
            return View(employeeListModel);
        }

        public async Task<IActionResult> Details(int? id)
        {

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);

        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel createViewModel)
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
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("details", "home", new { id = newEmployee.Id });
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
                employeeFromDb.Department = employee.Department;

                Console.WriteLine(employee.Email.ToString());
                Employee updatedEmployee = _employeeRepository.Update(employeeFromDb);
                return RedirectToAction("List");
            }
            return View(employee);
        }

        public ViewResult Delete(int id)
        {
            Employee employeeFromDb = _employeeRepository.GetEmployee(id);
            return View(employeeFromDb);
        }

        [HttpPost]
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
