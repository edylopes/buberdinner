

using System.Net.Mail;
using BuberDinner.Application.Common.Services;
using BuberDinner.Infrastructure.Services.SMTP.Configurations;



namespace BuberDinner.Infrastructure.Services.SMTP
{
    public static class DependsecyInjection
    {
        public static IServiceCollection AddSMTPService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<SmtpClientFactory>();
            services.AddTransient<IEmailService, SmtpEmailService>();


            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));

            services.AddTransient(sp =>
             {
                 var factory = sp.GetRequiredService<SmtpClientFactory>();
                 return factory.Create();
             });
            return services;
        }
    }
}