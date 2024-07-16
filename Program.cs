using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddMvc();
        builder.Services.AddDbContext<AppDbContext>(option =>
        option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

        var app = builder.Build();
        app.UseRouting();
        app.MapDefaultControllerRoute();
        app.UseStaticFiles();
        app.Run();
    }
}