using EmployeeManagement.Models.EmployeeManagement;

namespace EmployeeManagement.ViewModels
{
    public class HomeDetailsViewModel
    {
        // nullable or new Employee(); ? 
        public Employee Employee { get; set; }
        public string PageTitle {  get; set; } = string.Empty;
    }
}
