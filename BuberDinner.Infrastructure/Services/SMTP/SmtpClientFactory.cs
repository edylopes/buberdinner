
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using BuberDinner.Infrastructure.Services.SMTP.Configurations;


namespace BuberDinner.Infrastructure.Services.SMTP;


public class SmtpClientFactory
{
    private readonly SmtpOptions _options;
    public SmtpClientFactory(IOptions<SmtpOptions> options)
    {
        _options = options.Value;
    }

    public IOptions<SmtpOptions> Options { get; }

    public SmtpClient Create()
    {
        var client = new SmtpClient(_options.Host, _options.Port)
        {
            Credentials = new NetworkCredential(_options.User, _options.Password),
            EnableSsl = _options.UseSsl
        };

        return client;
    }
}
