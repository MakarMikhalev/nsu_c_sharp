using HackathonContract.ServiceContract;
using HackathonDatabase;
using HackathonDatabase.service;
using HackathonStrategy;
using HackthonEmployee;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace HackathonHrManager;

public class Program
{
    public static void Main()
    {
        CreateHostBuilder().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

                var connectionFactory = new ConnectionFactory
                {
                    HostName = "hackathon_rabbitmq",
                    UserName = "rabbit",
                    Password = "rabbit",
                    Port = 5672
                };

                ITeamBuildingStrategy teamBuildingStrategy = new TeamBuildingStrategy();
                var hrManager = new HrManager(teamBuildingStrategy);
                var hrManagerService = new HrManagerService(configuration, hrManager);
                var queueEmployee = configuration["RabbitMq:QueueEmployer"] ?? "QueueEmployee";
                services.AddHostedService<RabbitMqListener>(_ => new RabbitMqListener(queueEmployee, "hrManager", connectionFactory, hrManagerService));
                services.AddTransient<IEmployeeService, EmployeeService>();
                services.AddSingleton(hrManagerService);
            });    
}