
namespace BurberDinner.Infrastructure;

using BurberDinner.Infrastructure.Configuration;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.JWT));
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}