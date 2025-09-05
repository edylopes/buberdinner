using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Common.Errors;

internal sealed class ErrorDefaults
{
    public const string BadRequestUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
    public const string NotFoundUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    public const string ConflictUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    public const string UnauthorizedUrl = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
    public const string ForbiddenUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";

    public const int Conflict = 409;
    public const int Unauthorized = 401;
    public const int BadRequest = 400;
    public const int Notfound = 404;
    public const int Validation = 400;
}

public static class ErrorMapper
{

    public static (int statusCode, string url, string message) ToError(AppError error) =>
     error switch
     {
         DuplicatedEmailError => (StatusCodes.Status409Conflict, ErrorDefaults.ConflictUrl, error.Message),
         UserNotFoundError => (StatusCodes.Status404NotFound, ErrorDefaults.NotFoundUrl, error.Message),
         UserRoleNotAllowedError => (StatusCodes.Status403Forbidden, ErrorDefaults.ForbiddenUrl, error.Message),
         InvalidCredentialError => (StatusCodes.Status401Unauthorized, ErrorDefaults.UnauthorizedUrl, error.Message),

         _ => (StatusCodes.Status400BadRequest, ErrorDefaults.BadRequestUrl, "An unexpected error occurred")
     };
}
