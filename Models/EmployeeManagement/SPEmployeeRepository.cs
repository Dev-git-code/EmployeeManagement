using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmployeeManagement.Models.EmployeeManagement
{
    public class SPEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public SPEmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Employee?> Add(Employee employee,string p)
        {
            var parameters = new[]
            {
                new SqlParameter("@Name", employee.Name),
                new SqlParameter("@Email", employee.Email),
                new SqlParameter("@Department", employee.Department),
                new SqlParameter("@Role", employee.Role)
            };

            _context.Database.ExecuteSqlRaw("Exec dbo.spEmployee_Upsert @Name, @Email, @Department, @Role", parameters);

            return employee;
        }
        
        public Employee? Delete(int id)
        {
            Employee? employee = _context.Employees
                                .FromSqlRaw<Employee>("spEmployees_Get @Id", new SqlParameter("@Id", id))
                                .ToList()
                                .FirstOrDefault();

            if (employee != null)
            {
                var sql = "Exec dbo.spEmployee_Delete @EmpId";
                var parameter = new SqlParameter("@EmpId", id);
                _context.Database.ExecuteSqlRaw(sql, parameter);
            }

            return employee;

        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees.FromSqlRaw<Employee>("Exec dbo.spEmployees_Get");
        }

        public Employee? GetEmployee(int id)
        {

            var parameter = new SqlParameter("@Id", id);

            return _context.Employees
                .FromSqlRaw<Employee>("Exec dbo.spEmployees_Get @Id", parameter)
                .ToList()
                .FirstOrDefault();

        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            // var emailParam = new SqlParameter("@Email", SqlDbType.NVarChar, email)

            /*var employees = await _context.Employees
                .FromSqlRaw("spEmployees_Get {0}",email)
                .ToListAsync();

            return employees.FirstOrDefault();*/


            /*string sql = "EXEC dbo.spEmployees_Get @Email = ";
            sql += "'" + email + "'";

            return _context.Employees
                .FromSqlRaw(sql)
                .ToList().FirstOrDefault();*/

            var sql = "EXEC dbo.spEmployees_Get @Id, @Email";
            var parameters = new[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Id", DBNull.Value)
            };

            return _context.Employees
                .FromSqlRaw<Employee>(sql, parameters)
                .ToList()
                .FirstOrDefault();

        }

        public async Task<Employee> Update(Employee employeeUpdates)
        {
            var parameters = new[]
            {
                new SqlParameter("@Name", employeeUpdates.Name),
                new SqlParameter("@Email", employeeUpdates.Email),
                new SqlParameter("@Department", employeeUpdates.Department),
                new SqlParameter("@Role", employeeUpdates.Role),
                new SqlParameter("@Id", employeeUpdates.Id)
            };

            _context.Database.ExecuteSqlRaw("Exec dbo.spEmployee_Upsert @Name, @Email, @Department, @Role, @Id", parameters);

            return employeeUpdates;
        }
    }
}
