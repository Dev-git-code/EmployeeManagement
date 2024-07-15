
namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeesList;

        public MockEmployeeRepository()
        {
            _employeesList = new List<Employee>{
                new Employee() { Id = 1, Name = "Dev", Department = "Engineering", Email = "dev@accops.com" }, 
                new Employee() { Id = 2, Name = "Akash", Department = "Engineering", Email = "akash@accops.com" }, 
                new Employee() { Id = 3, Name = "Arjun", Department = "Engineering", Email = "arjun@accops.com" }
            };
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
    }
}
