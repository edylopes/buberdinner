using System.Net;
using System.Net.Mail;
using BuberDinner.Application.Common.Services;

namespace Infrastructure.Services;


public class SmtpEmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    public SmtpEmailService()
    {
        _smtpClient = new SmtpClient("smtp.server.com")
        {
            Credentials = new NetworkCredential("user", "password"),
            EnableSsl = true
        };
    }
    public async Task SendAsync(string to, string subject, string body)
    {

        var mail = new MailMessage("noreply@domain.com", to, subject, body);
        await _smtpClient.SendMailAsync(mail);
    }
}
