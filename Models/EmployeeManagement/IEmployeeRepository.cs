using System.Threading.Tasks;

namespace EmployeeManagement.Models.EmployeeManagement
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmployees();

        Task<Employee> GetEmployeeByEmailAsync(string email);
        Task<Employee> Add(Employee employee, string password);

        Task<Employee> Update(Employee employeeUpdates); 
        Employee Delete(int id);
    }
}
