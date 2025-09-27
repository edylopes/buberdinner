using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Common.Errors;

internal static class ErrorMapper
{
    public const string BadRequestUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
    public const string NotFoundUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    public const string ConflictUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    public const string UnauthorizedUrl = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
    public const string ForbiddenUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";

    public static (int statusCode, string url, string message) ToError(AppError error) =>
     error switch
     {
         DuplicatedEmailError => (StatusCodes.Status409Conflict, ConflictUrl, error.message),
         UserNotFoundError => (StatusCodes.Status404NotFound, NotFoundUrl, error.message),
         UserRoleNotAllowedError => (StatusCodes.Status403Forbidden, ForbiddenUrl, error.message),
         InvalidCredentialError => (StatusCodes.Status401Unauthorized, UnauthorizedUrl, error.message),

         _ => (StatusCodes.Status400BadRequest, BadRequestUrl, "An unexpected error occurred")
     };
}
