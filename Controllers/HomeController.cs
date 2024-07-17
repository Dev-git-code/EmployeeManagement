using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
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

        public ViewResult Details(int? id)
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
        public IActionResult Delete(Employee employee)
        {
            Employee employeeFromDb = _employeeRepository.GetEmployee(employee.Id);
            if(employeeFromDb != null)
            {
                Employee deletedEmployee = _employeeRepository.Delete(employee.Id);
                return RedirectToAction("Index");

            }
            return View(employee);
            
        }
    }
}
