
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Application.Common.Errors;

public record EmailAlreadyConfirmed(string url = TypeUrl.BadRequestUrl, string message = "Email is already confirmed")
  : AppError(400, url, message);
public record EmailConfirmationFailed(string url = TypeUrl.BadRequestUrl, string message = "Email onfirmation Failed", string title = "Validation Error")
  : AppError(400, url, message, title);

public record InvalidToken(string url = TypeUrl.BadRequestUrl, string message = "Invalid Token", string title = "Validation Error")
  : AppError(400, url, message);