using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models.EmployeeManagement
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SQLEmployeeRepository(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Employee?> Add(Employee employee, string password)
        {
           
            var user = new IdentityUser 
            {
                UserName = employee.Email,
                Email = employee.Email
            };

           
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                
                throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
          
            var roleResult = await _userManager.AddToRoleAsync(user, employee.Role.ToString());
            if (!roleResult.Succeeded)
            {            
                throw new Exception("Failed to assign role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }

           
            employee.IdentityUserId = user.Id;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

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
            return _context.Employees;
        }

        public Employee? GetEmployee(int id)
        {
            return _context.Employees.Find(id);
            
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            //return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
            
            var user = await _userManager.FindByEmailAsync(email);

            return await _context.Employees
                .FirstOrDefaultAsync(e => e.IdentityUserId == user.Id);
        }

        public async Task<Employee> Update(Employee employeeUpdates)
        {
            /* var employee = _context.Employees.Update(employeeUpdates);
             _context.SaveChanges();
             return employeeUpdates;*/

            /*
            var employee = _context.Employees.Attach(employeeUpdates);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return employeeUpdates;
            */

            
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == employeeUpdates.Id);

            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            
            existingEmployee.Name = employeeUpdates.Name;
            existingEmployee.Email = employeeUpdates.Email;
            existingEmployee.Department = employeeUpdates.Department;
            existingEmployee.Role = employeeUpdates.Role;

            
            _context.Employees.Update(existingEmployee);
            await _context.SaveChangesAsync();

            
            var user = await _userManager.FindByIdAsync(existingEmployee.IdentityUserId);
            if (user != null)
            {
                user.Email = employeeUpdates.Email;
                user.UserName = employeeUpdates.Email; 

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {                    
                    throw new Exception("Failed to update user details.");
                }
            }

            return existingEmployee;
        }
    }
}
