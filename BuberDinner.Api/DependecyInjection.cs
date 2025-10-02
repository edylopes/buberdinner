using AspNetCoreRateLimit;
using BuberDinner.Api.Common.Mapping;
using BuberDinner.Application;
using BuberDinner.Infrastructure.Authentication;
using BurberDinner.Infrastructure;



namespace BuberDinner.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStructureModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {

        //Add Mapping 
        services.AddMappings();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.JWT));

        //Add Modules
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);


        // Rate limit counter and rules store
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();

        return services;
    }
}
