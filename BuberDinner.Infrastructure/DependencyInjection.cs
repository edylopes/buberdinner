using BuberDinner.Application.Common.Interfaces.Persistence.Dinners;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Infrastructure.Authentication;
using BuberDinner.Infrastructure.Persistence;
using BuberDinner.Infrastructure.Persistence.Context;
using BuberDinner.Infrastructure.Services.SMTP;

namespace BuberDinner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)

    {
        serviceCollection.AddDbContext<AppDbContext>(opt
                    => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!));

        serviceCollection.AddAuthentication(configuration);
        serviceCollection.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        serviceCollection.AddSmtpService(configuration);

        serviceCollection.AddScoped<IUnitOfWork, EfUnitOfWork>();
        serviceCollection.AddScoped<IDinnerRepository, DinnerRepository>();
        //serviceCollection.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();


        return serviceCollection;
    }
}
