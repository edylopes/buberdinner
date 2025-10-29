

using BuberDinner.Contracts.Authentication.Email;

namespace BuberDinner.Application.Common.Interfaces.Services;

public interface IEmailService
{
    Task SendAsync(EmailMessage message);
}
