using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Models.EmployeeManagement;

public interface IApplicationUserRepository
{
    Task<ApplicationUser> GetUserByIdAsync(string id);
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
    Task<ApplicationUser> AddUserAsync(ApplicationUser user, string password);
    Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
    Task<ApplicationUser> DeleteUserAsync(string id);
}
