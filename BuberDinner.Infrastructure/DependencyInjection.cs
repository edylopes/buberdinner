
using BuberDinner.Infrastructure.Authentication;


namespace BurberDinner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.JWT));
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
