using EmployeeApp.Data.Data;
using EmployeeApp.Data.Interfaces.AddressRepo;
using EmployeeApp.Data.Interfaces.EmployeeRepo;
using EmployeeApp.PortalWithAuth.Controllers;
using EmployeeApp.Services.AddressServiceFolder;
using EmployeeApp.Services.EmployeeServiceFolder;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeeApp.PortalWithAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var adminUsers = new List<string> { "DESKTOP-49J7JJ2\\Titiksha", "DESKTOP-49J7JJ2\\HP" };
            builder.Services.AddSingleton(adminUsers);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60);
            });

            builder.Services.AddScoped<CustAuthAttribute>();
            
            builder.Services.AddLogging();
            builder.Services.AddControllers();
            builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
            builder.Services.AddDbContext<EmployeeDB2Context>(
            options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();

            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();

            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy.
                options.FallbackPolicy = options.DefaultPolicy;
            });
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSession();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc();

            app.Run();
        }
    }
}