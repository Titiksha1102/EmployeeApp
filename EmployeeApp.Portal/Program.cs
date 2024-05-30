using EmployeeApp.Portal;
using EmployeeApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using EmployeeApp.Data.Data;

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