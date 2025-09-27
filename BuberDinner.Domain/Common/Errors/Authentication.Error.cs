namespace BuberDinner.Domain.Common.Errors;

public record InvalidCredentialError()
    : AppError(

        "Invalid email or password"
    );
