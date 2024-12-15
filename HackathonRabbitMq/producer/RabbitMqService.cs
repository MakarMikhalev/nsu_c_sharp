namespace HackathonRabbitMq;

using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitMqService : IRabbitMqService
{
    
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _exchangeName;
    
    public RabbitMqService(ConnectionFactory factory, List<string> names, string exchangeName)
    {
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _exchangeName = exchangeName;

        _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
        foreach (var name in names)
        {
            _channel.QueueDeclare(queue: name,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.QueueBind(queue: name, exchange: exchangeName, routingKey: name);
        }
    }
    
    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending message: {message}");

        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: _exchangeName,
            routingKey: string.Empty,
            basicProperties: null,
            body: body);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
