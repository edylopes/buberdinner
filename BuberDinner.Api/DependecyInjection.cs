using AspNetCoreRateLimit;
using MediatR;

namespace BuberDinner.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPolicy(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddMemoryCache();
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

        // Registrando serviços necessários
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();

        return services;
    }
}
