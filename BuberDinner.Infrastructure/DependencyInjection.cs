using BuberDinner.Application.Common.Interfaces;
using BuberDinner.Application.Common.Interfaces.Persistence.Dinners;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Infrastructure.Authentication;
using BuberDinner.Infrastructure.Persistence;
using BuberDinner.Infrastructure.Persistence.Context;
using BuberDinner.Infrastructure.Messaging.RabbitMQ;

using BuberDinner.Infrastructure.Services.SMTP;

namespace BuberDinner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)

    {

        using var scope = services.BuildServiceProvider();
        var configuration = scope.GetRequiredService<IConfiguration>();

        //services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
        services.Configure<RabbitMqSettings>(configuration);
        services.AddDbContext<AppDbContext>(opt
                    => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!));

        services.AddAuthentication(configuration);
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddSmtpService(configuration);

        services.AddSingleton<IEmailQueue, RabbitMqEmailQueue>();

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IDinnerRepository, DinnerRepository>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));


        return services;
    }
}
