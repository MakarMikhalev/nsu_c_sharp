namespace HackathonRabbitMq;

public interface IRabbitMqService
{
    void SendMessage(string message);
}