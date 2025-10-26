

namespace BuberDinner.Domain.Common.Errors;



public static class TypeUrl
{

    private static readonly Dictionary<Type, AppError> Mappings = new();
    public const string BadRequestUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
    public const string NotFoundUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    public const string ConflictUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    public const string UnauthorizedUrl = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
    public const string ForbiddenUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
}



public abstract record AppError(int StatusCode, string Url, string Message, string? Title = null);

public record DuplicatedEmailError(string Url = TypeUrl.ConflictUrl, string Title = "Conflict Error")
    : AppError(409, Url, "User with email already exist", Title);
public record UserRoleNotAllowedError(string Url = TypeUrl.UnauthorizedUrl, string Title = "Unauthorized Error")
    : AppError(400, Url, "User Role not allowed", Title);
public record UserNotFoundError(string Url = TypeUrl.NotFoundUrl)
    : AppError(404, Url, "User notfound");

public record EmailNotConfirmedError(string Url = TypeUrl.BadRequestUrl)
    : AppError(400, Url, "Email is not confirmed");




