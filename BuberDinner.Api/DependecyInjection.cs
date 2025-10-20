
using AspNetCoreRateLimit;
using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Common.Mapping;
using BuberDinner.Application;
using BuberDinner.Application.Common.Services;
using BuberDinner.Infrastructure;
using BuberDinner.Infrastructure.Authentication;
using BuberDinner.Infrastructure.Services.SMTP.Configurations;

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
        services.AddHttpContextAccessor();

        services.AddInfrastructureServices(configuration);

        services.AddApplicationServices();
        // JWT Configuration
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.JWT));

        // Rate limit counter and rules store
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();

        //Smtp Email Service Configuration (errado) cria container Paralelo
        /*    using var serviceProvider = services.BuildServiceProvider();
              using var smtpClient = serviceProvider
                                   .GetRequiredService<SmtpClientFactory>()
                                   .Create(); 
        */
        var smtpConfig = configuration.GetSection(nameof(SmtpOptions)).Get<SmtpOptions>()!;

        services
          .AddFluentEmail(smtpConfig.FromEmail, smtpConfig.FromName)
          .AddSmtpSender(smtpConfig.Host, smtpConfig.Port, smtpConfig.Username, smtpConfig.Password)
          .AddRazorRenderer();

        return services;
    }

    public static IApplicationBuilder UseApiConfigurations(this IApplicationBuilder app)
    {
        var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
        ErrorResults.Configure(httpContextAccessor);

        using var scope = app.ApplicationServices.CreateScope();
        //TESTE DE ENVIO DE EMAIL
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("SMTP Configuration");

        var email = scope.ServiceProvider.GetRequiredService<IEmailService>();

        // await email.SendAsync("fenderlopes@gmail.com", "Test Email Buber Dinner", "WelcomeEmail.cshtml", "Ednei", Guid.NewGuid());
        // logger.LogInformation("✅ E-mail de teste enviado com sucesso!");


        return app;
    }
}
