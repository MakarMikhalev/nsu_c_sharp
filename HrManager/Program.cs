using HackathonDatabase;
using HackathonDatabase.service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HackathonHrManager;

public class Program
{
    public static void Main()
    {
        CreateHostBuilder().Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices(services =>
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var serviceProvider = new ServiceCollection()
                    .AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
                    .BuildServiceProvider();

                var context = serviceProvider.GetService<ApplicationDbContext>();

                services.AddTransient<HrManager>();
                services.AddTransient<HrManagerController>();
                services.AddTransient<EmployeeService>();
                services.AddTransient<HrManagerService>();
                services.AddSingleton(context);
            });
}