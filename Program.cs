using EmployeeManagement.Models.EmployeeManagement;
using EmployeeManagement.Models.RoleManagement;
using EmployeeManagement.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddMvc();
        builder.Services.AddDbContext<AppDbContext>(option =>
        option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();

        var app = builder.Build();
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseStatusCodePagesWithRedirects("/Error/{0}");
        }
        app.UseStaticFiles();
        app.UseRouting();
        app.MapDefaultControllerRoute();
       
        app.UseAuthentication();
        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await CreateRoles.CreateRolesAsync(roleManager);
        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
            await SuperAdmin.CreateSuperAdmin(userManager, employeeRepository); 
        }
     
        app.Run();

    }
}