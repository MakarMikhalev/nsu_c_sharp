using HackathonHrDirector;
using HackathonHrManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HackathonRunner;
using HackathonContract.ServiceContract;
using HackathonDatabase;
using HackathonDatabase.service;
using HackathonStrategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HackathonEveryone;

public static class Program
{
    public static void Main()
    {
        CreateHostBuilder().Build().Run();
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

                services.AddTransient<Hackathon>();
                services.AddTransient<ITeamBuildingStrategy, TeamBuildingStrategy>();
                services.AddTransient<HrManager>();
                services.AddTransient<HrDirector>();
                services.AddTransient<IHackathonService, HackathonService>();
                services.AddTransient<IEmployeeService, EmployeeService>();
                services.AddTransient<HackathonRunner>();
                services.AddHostedService<HackathonWorker>();
                services.AddSingleton(context);
            });
}