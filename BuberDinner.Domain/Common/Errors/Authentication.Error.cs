namespace BuberDinner.Domain.Common.Errors;

public record InvalidCredentialError()
    : AppError(
        ErrorDefaults.NoAuthorized,
        ErrorDefaults.BadRequestUrl,
        "Invalid email or password"
    );
