using System.Threading.Tasks;

namespace EmployeeManagement.Models.EmployeeManagement
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmployees();

        Task<Employee> GetEmployeeByEmailAsync(string email);
        Employee Add(Employee employee);

        Employee Update(Employee employeeUpdates);
        Employee Delete(int id);
    }
}
