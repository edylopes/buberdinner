

namespace BuberDinner.Contracts.Authentication.Email;

public record EmailMessage(string to, string subject, string body, string name, Guid userId);