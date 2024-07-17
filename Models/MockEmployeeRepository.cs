

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeesList;

        public MockEmployeeRepository()
        {
            _employeesList = new List<Employee>{
                new Employee() { Id = 1, Name = "Vipin", Department = Department.Customer_Support, Email = "vipin@accops.com" }, 
                new Employee() { Id = 2, Name = "Rajiv", Department = Department.Sales, Email = "rajiv@accops.com" }, 
                new Employee() { Id = 3, Name = "Dev", Department = Department.IT_Infra, Email = "dev@accops.com" }, 
                new Employee() { Id = 4, Name = "Kartik", Department = Department.Sales, Email = "kartik@accops.com" }, 
                new Employee() { Id = 5, Name = "Samarth", Department = Department.HR, Email = "samarth@accops.com" }, 
                new Employee() { Id = 6, Name = "Akash", Department = Department.Engineering, Email = "akash@accops.com" }, 
                new Employee() { Id = 7, Name = "Arjun", Department = Department.IT_Infra, Email = "arjun@accops.com" },
                new Employee() { Id = 8, Name = "Risabh Rai", Department = Department.Customer_Support, Email = "risabh.rai@accops.com" }
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeesList.Max(x => x.Id) + 1;
            _employeesList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeesList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeesList.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeesList;
        }

        public Employee GetEmployee(int id)
        {
            Employee? employee = this._employeesList.FirstOrDefault(e => e.Id == id);
            return employee ?? new Employee();
            
        }

        public Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Employee Update(Employee employeeUpdates)
        {
            Employee employee = _employeesList.FirstOrDefault(e => e.Id == employeeUpdates.Id);
            if (employee != null)
            {
                employee.Name = employeeUpdates.Name;
                employee.Email = employeeUpdates.Email;
                employee.Department = employeeUpdates.Department;
            }
            return employee;
        }
    }
}
