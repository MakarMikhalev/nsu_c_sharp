using HackathonHrDirector;
using HackathonHrManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HackathonRunner;
using HackathonContract.ServiceContract;
using HackathonStrategy;

namespace HackathonEveryone;

public static class Program
{
    public static void Main()
    {
        System.Console.WriteLine("Hello world!");
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
                services.AddTransient<Hackathon>(_ => new Hackathon());
                services.AddTransient<ITeamBuildingStrategy, TeamBuildingStrategy>();
                services.AddTransient<HrManager>();
                services.AddTransient<HrDirector>();
                services.AddHostedService<HackathonWorker>();
            });
}