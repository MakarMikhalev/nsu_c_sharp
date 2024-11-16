using System.Net;

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
        CreateHostBuilder().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseKestrel(options => { options.Listen(IPAddress.Any, 8080); })
                    .ConfigureAppConfiguration(config =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.json", optional: true,
                            reloadOnChange: true);
                    })
                    .ConfigureAppConfiguration(config =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.json", optional: true,
                            reloadOnChange: true);
                    })
                    .ConfigureServices(services =>
                    {
                        var configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();

                        var serviceProvider = new ServiceCollection()
                            .AddDbContext<ApplicationDbContext>(options =>
                                options.UseNpgsql(
                                    configuration.GetConnectionString("DefaultConnection")))
                            .BuildServiceProvider();

                        var context = serviceProvider.GetService<ApplicationDbContext>();

                        services.AddTransient<IEmployeeService, EmployeeService>();
                        services.AddTransient<IHackathonService, HackathonService>();
                        services.AddTransient<HrDirector>();
                        services.AddTransient<HrDirectorService>();
                        services.AddTransient<HrDirectorController>();
                        services.AddSingleton(context);
                        services.AddControllers();
                    })
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                    });
            });
}