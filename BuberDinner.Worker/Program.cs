using BuberDinner.Application.Common.Interfaces;
using BuberDinner.Application.Common.Interfaces.Services;
using BuberDinner.Infrastructure.Messaging.RabbitMQ;
using BuberDinner.Infrastructure.Services.SMTP;
using BuberDinner.Worker;


var host = Host.CreateDefaultBuilder(args)
     .ConfigureAppConfiguration((_, config) =>
    {
        // Isso garante que o .env e variáveis do Docker sejam lidos
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {

        services.Configure<RabbitMqSettings>(context.Configuration);//GetSection não funciona para .env
        services.AddSingleton<IEmailQueue, RabbitMqEmailQueue>();

        services.AddHostedService<EmailWorkerService>();

        services.AddTransient<IEmailService, SmtpEmailService>();


    })
    .Build();

host.Run();
