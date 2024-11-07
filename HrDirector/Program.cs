namespace HackathonHrDirector;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HackathonDatabase;
using HackathonDatabase.service;
using Microsoft.EntityFrameworkCore;

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

                services.AddTransient<EmployeeService>();
                services.AddTransient<HackathonService>();
                services.AddTransient<HrDirector>();
                services.AddTransient<HrDirectorService>();
                services.AddTransient<HrDirectorController>();
                services.AddSingleton(context);
            });
}