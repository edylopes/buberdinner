namespace BuberDinner.Domain.Common.Errors;

public record InvalidCredentialError(string url = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1", string title = "Validation Error")
    : AppError(400, url, "Invalid email or password", title);
