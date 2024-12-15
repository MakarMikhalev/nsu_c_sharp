using HackathonRabbitMq;

namespace HackthonEmployee;

using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Text;

public class RabbitMqListener : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceRunnable _serviceRunnable;

    public RabbitMqListener(string exchange, string queueName, ConnectionFactory connectionFactory,
        IServiceRunnable serviceRunnable)
    {
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _serviceRunnable = serviceRunnable;
        
        _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);
        _channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        _channel.QueueBind(queue: queueName, exchange: exchange, routingKey: "");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        Console.WriteLine("Listening for RabbitMQ messages on fanout exchange...");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            Console.WriteLine($"Receiving message: {content}");
            _serviceRunnable.Run(content);

            _channel.BasicAck(ea.DeliveryTag, true);
        };

        _channel.BasicConsume(queue: _channel.QueueDeclare().QueueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}