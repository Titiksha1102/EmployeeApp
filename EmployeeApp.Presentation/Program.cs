using EmployeeApp.Services;
using EmployeeApp.Data.Interfaces;
using EmployeeApp.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
/*internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<EmployeeDbContext>(
            options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        var app = builder.Build();
        Console.WriteLine("building once again");

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            Console.WriteLine("is dev env");
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        Console.WriteLine("pipeline started");
        app.UseStaticFiles();
        Console.WriteLine("static files rendered");
        app.Use(async (context, next) =>
        {
            Console.WriteLine("inside pipeline");
            return await next();
        });
        Console.WriteLine("after use");
        app.UseMvc();
    }
}*/
namespace EmployeeApp.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                });
    }
}
