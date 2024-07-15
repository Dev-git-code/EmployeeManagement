using EmployeeManagement.Models;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddMvc();
        builder.Services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();

        var app = builder.Build();
        app.UseRouting();
        app.MapDefaultControllerRoute();
        app.Run();
    }
}