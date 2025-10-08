using BuberDinner.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace BurberDinner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)

    {

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();


        return services;
    }
}
