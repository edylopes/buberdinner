

namespace BuberDinner.Domain.Common.Errors;



public static class TypeUrl
{

    private static readonly Dictionary<Type, AppError> _mappings = new();
    public const string BadRequestUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
    public const string NotFoundUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    public const string ConflictUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    public const string UnauthorizedUrl = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
    public const string ForbiddenUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
}



public abstract record AppError(int statusCode, string url, string message, string? title = null);

public record DuplicatedEmailError(string url = TypeUrl.ConflictUrl)
    : AppError(409, url, "User with email already exist");
public record UserRoleNotAllowedError(string url = TypeUrl.UnauthorizedUrl)
    : AppError(400, url, "User Role not allowed");
public record UserNotFoundError(string url = TypeUrl.NotFoundUrl)
    : AppError(404, url, "User notfound");


