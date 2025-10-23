
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Application.Common.Errors;

public record EmailAlreadyConfirmed(string Url = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4", string Message = "Email is already confirmed", string Title = "Validation Error")
  : AppError(400, Url, Message, Title);
public record EmailConfirmationFailed(string Url = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1", string Title = "Validation Error")
  : AppError(400, Url, "Email confirmation failed", Title);

