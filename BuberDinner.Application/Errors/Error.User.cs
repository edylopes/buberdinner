namespace BuberDinner.Application.Errors;

/// <summary>
/// Contains errors definitions related to user operations.
/// </summary>
///
///
public static class ErrorDefaults
{
    public const string TypeUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
    public const string TypeUrlNotFound =
        "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    public const int Conflict = 409;
    public const int BadRequest = 400;
    public const int Notfound = 400;
}

public abstract record AppError(int StatusCode, string TypeUrl, string Message);

public record DuplicatedEmailError()
    : AppError(ErrorDefaults.Conflict, ErrorDefaults.TypeUrl, "User with email already exist");

public record UserRoleNotAllowedError()
    : AppError(ErrorDefaults.BadRequest, ErrorDefaults.TypeUrl, "User Role not allowed");

public record InvalidCredentialError()
    : AppError(ErrorDefaults.BadRequest, ErrorDefaults.TypeUrl, "Invalid email or password");

public record UserNotFoundError()
    : AppError(ErrorDefaults.BadRequest, ErrorDefaults.TypeUrl, "User not found");
