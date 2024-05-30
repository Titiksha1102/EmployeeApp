using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Data.Data;
using EmployeeApp.Data.Interfaces.EmployeeRepo;
using EmployeeApp.Data.Interfaces.AddressRepo;
using EmployeeApp.Services.EmployeeServiceFolder;
using EmployeeApp.Services.AddressServiceFolder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

namespace EmployeeApp.Portal
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddControllers();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddDbContext<EmployeeDB2Context>(
            options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
           
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAddressRepository, AddressRepository>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseStaticFiles();
            app.Use((context, next) =>
            {
                Console.WriteLine(context.Request.Path);
                
                return next();
            });

            app.UseMvc();
        }

    }
}
