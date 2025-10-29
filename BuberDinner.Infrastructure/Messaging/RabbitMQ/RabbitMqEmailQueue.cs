
using System.Text;
using System.Text.Json;

using BuberDinner.Application.Common.Interfaces;
using BuberDinner.Contracts.Authentication.Email;

using Microsoft.Extensions.Options;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace BuberDinner.Infrastructure.Messaging.RabbitMQ;

public class RabbitMqEmailQueue : IEmailQueue, IDisposable
{
    private IConnection? _connection;
    private readonly IServiceProvider _provider;
    private readonly IOptions<RabbitMqSettings> _options;
    private IModel? _channel;
    private readonly string _queueName = "email_queue";
    public RabbitMqEmailQueue(IOptions<RabbitMqSettings> options, IServiceProvider provider)
    {
        _options = options;
        _provider = provider;

        Console.WriteLine("RabbitMqSettings:");
        Console.WriteLine($"HostName: {_options.Value.HostName}");
        Console.WriteLine($"UserName: {_options.Value.UserName}");
        Console.WriteLine($"Password: {_options.Value.Password}");
        Console.WriteLine($"Port: {_options.Value.Port}");

    }


    public void Connect()
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Value.HostName,
            UserName = _options.Value.UserName,
            Password = _options.Value.Password,
            Port = _options.Value.Port
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);
    }
    public ValueTask EnqueueAsync(EmailMessage message, CancellationToken ct)
    {


        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true; // mensagem persistente no disco

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.
            BasicPublish(exchange: "",
                              routingKey: _queueName,
                              basicProperties: null,
                              body: body);

        return ValueTask.CompletedTask;
    }

    public void StartConsuming(CancellationToken ct)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        Console.WriteLine("✅ Worker conectado ao RabbitMQ e pronto para consumir");

        consumer.Received += async (ch, ea) =>
        {
            Console.WriteLine("📦 Mensagem recebida!");
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<EmailMessage>(json)!;


                // Cria escopo pra acessar serviços do DI
                using var scope = _provider.CreateScope();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailService>();

                await emailSender.SendAsync(message);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception)
            {
                _channel.BasicNack(ea.DeliveryTag, false, requeue: true);
            }
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
