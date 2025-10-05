using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Common.Errors;

internal static class ErrorMapper
{
    private static readonly Dictionary<Type, (int statusCode, string url, string message, string title)> _mappings = new();
    public const string BadRequestUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
    public const string NotFoundUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    public const string ConflictUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    public const string UnauthorizedUrl = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
    public const string ForbiddenUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";


    public static void Register(AppError error)

    {
        var type = error.GetType();
        _mappings[type] = (error.statusCode, error.url, error.message, error.title!);

    }
    public static (int statusCode, string url, string message, string title)? GetMapping(AppError error)
    {

        Register(error);
        var type = error.GetType();
        return _mappings.TryGetValue(type, out var mapping) ? mapping : null;
    }

}
