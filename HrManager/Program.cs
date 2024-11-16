using System.Net;
using HackathonContract.ServiceContract;
using HackathonDatabase;
using HackathonDatabase.service;
using HackathonStrategy;
using Microsoft.EntityFrameworkCore;

namespace HackathonHrManager;

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
                webBuilder.UseKestrel(options => { options.Listen(IPAddress.Any, 8081); })
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

                        services.AddTransient<HrManager>();
                        services.AddTransient<HrManagerController>();
                        services.AddTransient<IEmployeeService, EmployeeService>();
                        services.AddSingleton<HrManagerService>();
                        services.AddTransient<ITeamBuildingStrategy, TeamBuildingStrategy>();
                        services.AddSingleton(context);
                        services.AddControllers();
                    })
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
            });
}