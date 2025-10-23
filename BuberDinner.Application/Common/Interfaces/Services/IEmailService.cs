

namespace BuberDinner.Application.Common.Interfaces.Services;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string templateName, string name, Guid userId);
}
