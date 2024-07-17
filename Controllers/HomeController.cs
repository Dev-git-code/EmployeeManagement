using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(IEmployeeRepository employeeRepository,
            UserManager<IdentityUser> userManager)
        {
            this._employeeRepository = employeeRepository;
            _userManager = userManager;
        }

        public ViewResult Index()
        {
            var employeeListModel = _employeeRepository.GetAllEmployees();
            return View(employeeListModel);
        }

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
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee newEmployee = _employeeRepository.Add(employee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }

            return View();
                

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
                return RedirectToAction("Index");
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
            //var user = await _userManager.FindByEmailAsync(employee.Email);
            
            if(employeeFromDb != null )
                //&& user!=null)
            {
               // var employeeFromUserDb = await _userManager.DeleteAsync(user);
                Employee deletedEmployee = _employeeRepository.Delete(employee.Id);
                return RedirectToAction("Index");
            }
            return View(employee);
            
        }
    }
}
