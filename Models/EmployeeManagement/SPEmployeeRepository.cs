using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models.EmployeeManagement
{
    public class SPEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public SPEmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public Employee Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Employee? Delete(int id)
        {
            Employee employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees.FromSqlRaw<Employee>("spEmployees_Get");
        }

        public Employee? GetEmployee(int id)
        {
            
            return _context.Employees.FromSqlRaw<Employee>("spEmployees_Get {0}", id)
                .ToList().FirstOrDefault();
        }

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public Employee Update(Employee employeeUpdates)
        {
            var employee = _context.Employees.Update(employeeUpdates);
            _context.SaveChanges();
            return employeeUpdates;

            /*
            var employee = _context.Employees.Attach(employeeUpdates);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return employeeUpdates;
            */
        }
    }
}
