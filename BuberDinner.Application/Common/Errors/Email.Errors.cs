
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Application.Common.Errors;

public record EmailAlreadyConfirmed(string url = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4", string message = "Email is already confirmed", string title = "Validation Error")
  : AppError(400, url, message, title);
public record EmailConfirmationFailed(string url = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1", string title = "Validation Error")
  : AppError(400, url, "Email confirmation failed", title);

