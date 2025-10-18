using BuberDinner.Application.Common.Interfaces.Persistence.Dinners;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Infrastructure.Persistence.Repositories.Context;
using BuberDinner.Infrastructure.Services.SMTP;

namespace BuberDinner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)

    {

        serviceCollection.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        serviceCollection.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!));
        serviceCollection.AddScoped<IUnitOfWork, EfUnitOfWork>();
        serviceCollection.AddScoped<IDinnerRepository, DinnerRepository>();
        //serviceCollection.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();


        serviceCollection.AddSMTPService(configuration);


        return serviceCollection;
    }
}
