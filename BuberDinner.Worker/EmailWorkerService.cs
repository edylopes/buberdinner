
using BuberDinner.Application.Common.Interfaces;

namespace BuberDinner.Worker;

public class EmailWorkerService : BackgroundService
{
    private readonly ILogger<EmailWorkerService> _logger;
    private readonly IServiceProvider _provider;

    private readonly IEmailQueue _emailQueue;

    public EmailWorkerService(
        IEmailQueue emailQueue,
        IServiceProvider provider,
        ILogger<EmailWorkerService> logger)
    {
        _emailQueue = emailQueue;
        _provider = provider;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Sending email to new user");
        //Connect to RabbitMQ|Broker
        _emailQueue.Connect();
        // Inicia o consumo de mensagens
        _emailQueue.StartConsuming(stoppingToken);
        // O worker não precisa de loop adicional, o RabbitMQ chama o callback automaticamente
        return Task.CompletedTask;
    }

}
