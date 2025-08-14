using System.Globalization;

namespace BuberDinner.Domain.Common.Errors;

internal sealed class ErrorDefaults
{
    public const string BadRequestUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
    public const string NotFoundUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    public const string ConflictUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    public const string ForbiddenUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";

    public const int Conflict = 409;
    public const int NoAuthorized = 401;
    public const int BadRequest = 400;
    public const int Notfound = 404;
    public const int Validation = 400;
}

public abstract record AppError(int StatusCode, string TypeUrl, string Message);

public record DuplicatedEmailError()
    : AppError(ErrorDefaults.Conflict, ErrorDefaults.ConflictUrl, "User with email already exist");

public record UserRoleNotAllowedError()
    : AppError(ErrorDefaults.BadRequest, ErrorDefaults.BadRequestUrl, "User Role not allowed");

public record UserNotFoundError()
    : AppError(ErrorDefaults.Notfound, ErrorDefaults.NotFoundUrl, "User not found");
