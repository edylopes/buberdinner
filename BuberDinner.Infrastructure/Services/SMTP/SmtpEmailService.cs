using BuberDinner.Application.Common.Services;
using BuberDinner.Infrastructure.Services.SMTP.Configurations;
using Microsoft.Extensions.Options;
using FluentEmail.Core;

using BuberDinner.Infrastructure.Services.SMTP.Model;

namespace BuberDinner.Infrastructure.Services.SMTP;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpOptions _options;
    private readonly IFluentEmail _fluentEmail;
    public SmtpEmailService(IOptions<SmtpOptions> options, IFluentEmail fluentEmail)
    {
        _options = options.Value;
        _fluentEmail = fluentEmail;
    }
    public async Task SendAsync(string to, string subject, string templateName, string name, Guid userId)
    {
        var model = new WelcomeEmail
        {
            UserName = name,
            ConfirmLink = $"https://localhost:7104/api/v1/confirm?token={userId}"
        };
        var templatePath = Path.Combine(AppContext.BaseDirectory, "Services/SMTP/Templates", $"{templateName}");

        await _fluentEmail
            .To(to)
            .SetFrom(_options.FromEmail, _options.FromName)
            .ReplyTo(_options.FromEmail)
            .Subject(subject)
            .UsingTemplateFromFile(templatePath, model, isHtml: true)
            .SendAsync();
    }
}
