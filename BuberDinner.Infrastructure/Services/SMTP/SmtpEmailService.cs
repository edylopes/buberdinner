using BuberDinner.Infrastructure.Services.SMTP.Configurations;
using Microsoft.Extensions.Options;
using FluentEmail.Core;

using BuberDinner.Infrastructure.Services.SMTP.Model;

namespace BuberDinner.Infrastructure.Services.SMTP;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpOptions _options;
    private readonly IFluentEmail _fluentEmail;

    private readonly IJwtTokenGenerator jwtTokenGenerator;
    public SmtpEmailService(IOptions<SmtpOptions> options, IFluentEmail fluentEmail, IJwtTokenGenerator jwtTokenGenerator)
    {
        this._options = options.Value;
        this._fluentEmail = fluentEmail;
        this.jwtTokenGenerator = jwtTokenGenerator;
    }
    public async Task SendAsync(string to, string subject, string templateName, string name, Guid userId)
    {

        string token = jwtTokenGenerator.GenerateEmailConfirmationToken(userId.ToString());
        var model = new WelcomeEmail
        {

            UserName = name,
            ConfirmLink = $"https://frontend:7104/api/v1/confirm-email?token={token}"
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
