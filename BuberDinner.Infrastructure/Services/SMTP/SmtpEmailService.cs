using BuberDinner.Infrastructure.Services.SMTP.Configurations;
using Microsoft.Extensions.Options;
using FluentEmail.Core;

using BuberDinner.Infrastructure.Services.SMTP.Model;
using BuberDinner.Contracts.Authentication.Email;

namespace BuberDinner.Infrastructure.Services.SMTP;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpOptions _options;
    private readonly IFluentEmail _fluentEmail;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    public SmtpEmailService(IOptions<SmtpOptions> options, IFluentEmail fluentEmail, IJwtTokenGenerator jwtTokenGenerator)
    {
        this._options = options.Value;
        this._fluentEmail = fluentEmail;
        this._jwtTokenGenerator = jwtTokenGenerator;
    }
    public async Task SendAsync(EmailMessage message)
    {

        var token = _jwtTokenGenerator.GenerateEmailConfirmationToken(message.userId.ToString());
        var model = new WelcomeEmail
        {

            UserName = message.name,
            ConfirmLink = $"https://frontend.com.br:7104/api/v1/confirm-email?token={token}"
        };
        var templatePath = Path.Combine(AppContext.BaseDirectory, "Services/SMTP/Templates", $"{message.body}");

        await _fluentEmail
            .To(message.to)
            .SetFrom(_options.FromEmail, _options.FromName)
            .ReplyTo(_options.FromEmail)
            .Subject(message.subject)
            .UsingTemplateFromFile(templatePath, model, isHtml: true)
            .SendAsync();
    }
}
