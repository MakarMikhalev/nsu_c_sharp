using HackathonRabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace HackthonEmployee;

public class Program
{
    public static void Main()
    {
        CreateHostBuilder().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder()
        =>
        Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: true,
                    reloadOnChange: true);
            })
            .ConfigureServices(services =>
            {
                var connectionFactory = new ConnectionFactory { HostName = "hackathon_rabbitmq", UserName = "rabbit", Password = "rabbit", Port = 5672 };
                
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var queueEmployee = configuration["RabbitMq:QueueEmployer"] ?? "QueueEmployer";
                var queueName = Environment.GetEnvironmentVariable("HR_MANAGER_QUEUE") ?? "hr-manager";
                
                var rabbitMqService = new RabbitMqService(connectionFactory, [queueName], queueEmployee);
                var employerService = new EmployerService(rabbitMqService);
                var queueMetaInfo = configuration["RabbitMq:QueueMetaInfo"] ?? "QueueMetaInfo";
                
                var type = Environment.GetEnvironmentVariable("EMPLOYER_TYPE")?.ToUpper();
                var employeeId = Environment.GetEnvironmentVariable("EMPLOYER_ID");
                var topicExchange = $"{type}-{employeeId}";
                Console.WriteLine($"Employee topic exchange: {topicExchange}");
                services.AddHostedService<RabbitMqListener>(_ => new RabbitMqListener(queueMetaInfo, topicExchange, connectionFactory, employerService));
            });
}