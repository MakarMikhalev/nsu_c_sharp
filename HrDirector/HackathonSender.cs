using HackathonRabbitMq;

namespace HackathonHrDirector;

public class HackathonSender(IRabbitMqService rabbitMqService) : IHostedService
{
    private readonly int countHackathon = 10;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {   
        Console.WriteLine("HackathonSender starting...");
        for (var i = 0; i < countHackathon; ++i)
        {
            rabbitMqService.SendMessage(i.ToString());
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}