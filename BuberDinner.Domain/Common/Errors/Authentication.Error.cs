namespace BuberDinner.Domain.Common.Errors;

public record InvalidCredentialError(string Url = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1", string Title = "Validation Error")
    : AppError(400, Url, "Invalid email or password", Title);
