
namespace BuberDinner.Infrastructure.Services.SMTP.Configurations;

public class SmtpOptions
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool UseSsl { get; set; } = false;
    public string FromEmail { get; set; } = null!;
    public string FromName { get; set; } = null!;
}