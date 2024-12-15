using System.Net;
using HackathonEveryone.Utils;
using HackathonRabbitMq;
using RabbitMQ.Client;

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
                        
                        var connectionFactory = new ConnectionFactory { HostName = "hackathon_rabbitmq", UserName = "rabbit", Password = "rabbit", Port = 5672 };
                        
                        var context = serviceProvider.GetService<ApplicationDbContext>();
                        services.AddTransient<IEmployeeService, EmployeeService>();
                        services.AddTransient<IHackathonService, HackathonService>();
                        
                        var queueNames = ParseCsv.RunAsyncNamesQueue(configuration["HackathonSettings:JuniorFile"], configuration["HackathonSettings:TeamLeadFile"]);
                        Console.WriteLine($"Queue Names: {string.Join(", ", queueNames)}");
                        
                        var rabbitMqService = new RabbitMqService(connectionFactory, queueNames, configuration["RabbitMq:QueueMetaInfo"]);
                        services.AddSingleton<IRabbitMqService>(rabbitMqService);
                        services.AddTransient<HrDirector>();
                        services.AddTransient<HrDirectorService>();
                        services.AddTransient<HrDirectorController>();
                        services.AddTransient<HackathonService>();
                        services.AddHostedService<HackathonSender>();
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