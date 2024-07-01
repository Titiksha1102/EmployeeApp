using EmployeeApp.Data.Data;
using EmployeeApp.Data.Interfaces.AddressRepo;
using EmployeeApp.Data.Interfaces.GroupRepo;
using EmployeeApp.Data.Interfaces.UserRepo;
using EmployeeApp.PortalWithAuth.Controllers;
using EmployeeApp.ServiceApi.Controllers.AdressesService;
using EmployeeApp.ServiceApi.Controllers.GroupsService;
using EmployeeApp.ServiceApi.Controllers.UsersService;
using EmployeeApp.Services.AddressServiceFolder;
using EmployeeApp.Services.UserServiceFolder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EmployeeApp.PortalWithAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           /* // Add services to the container.
            var adminUsers = new List<string> { "DESKTOP-49J7JJ2\\Titiksha", "DESKTOP-49J7JJ2\\HP" };
            builder.Services.AddSingleton(adminUsers);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60);
            });

            builder.Services.AddScoped<CustAuthAttribute>();*/
            
            builder.Services.AddLogging();
            builder.Services.AddControllersWithViews();
            /*builder.Services.AddMvc(options => options.EnableEndpointRouting = false);*/
            builder.Services.AddDbContext<EmployeeDB2Context>(
            options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddScoped<IUsersService, UsersServiceController>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAddressesService, AddressesServiceController>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<IGroupsService, GroupsServiceController>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
                
            })
            .AddCookie("cookie")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:7115";
                options.ClientId = "oidcMVCApp";
                options.ClientSecret = "ProCodeGuide";
                options.ResponseType = "code";
                options.UsePkce = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ResponseMode = "query";
                options.Scope.Add("weatherApi.read");
                options.SaveTokens = true;
                options.SignedOutCallbackPath = "/signout-callback-oidc";
                options.SignedOutRedirectUri = "https://localhost:7011/";
                options.Scope.Add("role");
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("CustomClaim");
                options.ClaimActions.MapJsonKey("role", "role", "role");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                
                options.AddPolicy("CustomPolicy", policy =>
                {
                    policy.RequireClaim("role", "admin");
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            /*app.UseMvc();*/
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Landing}/{id?}");
                endpoints.MapControllerRoute(
                    name: "signout-callback-oidc",
                    pattern: "signout-callback-oidc",
                    defaults: new { controller = "Home", action = "LogoutCallback" });
            });

            app.Run();
        }
    }
}