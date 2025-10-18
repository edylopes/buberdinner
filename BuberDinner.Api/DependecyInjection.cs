
using System.Net;
using System.Net.Mail;
using AspNetCoreRateLimit;
using BuberDinner.Api.Common.Mapping;
using BuberDinner.Application;
using BuberDinner.Infrastructure;
using BuberDinner.Infrastructure.Authentication;
using BuberDinner.Infrastructure.Services.SMTP;
using FluentEmail.Smtp;


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

        services.AddInfrastructureServices(configuration);

        services.AddApplicationServices();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.JWT));
        //Add Modules

        // Rate limit counter and rules store
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();


        //Smtp Email Service Configuration
        services
          .AddFluentEmail(configuration["SmtpOptions:FromEmail"], configuration["SmtpOptions:FromName"])
          .AddSmtpSender(
              configuration["SmtpOptions:Host"],
              int.Parse(configuration["SmtpOptions:Port"]!),
              configuration["SmtpOptions:Username"],
              configuration["SmtpOptions:Password"])
        .AddRazorRenderer();
        return services;
    }
}
