using AspNetCoreRateLimit;
using BuberDinner.Api.Common.Mapping;
using BuberDinner.Application;
using BuberDinner.Infrastructure.Configuration;
using BurberDinner.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BuberDinner.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStructureModule(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {

        //Add Mapping 
        services.AddMappings();
        //Add Modules
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);
        
        //Configurations
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.JWT));

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
