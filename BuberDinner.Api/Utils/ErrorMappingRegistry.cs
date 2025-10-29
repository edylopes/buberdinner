
namespace BuberDinner.Api.Utils;

using BuberDinner.Domain.Common.Errors;
public static class ErrorMappingRegistry
{
    const string ConflictUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    const string ForbiddenUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";

    private static readonly Dictionary<Type, (int statusCode, string title, string typeUrl)> _mappings = new();
    public static void Register<TError>(int statusCode, string title, string typeUrl)
        where TError : AppError
    {
        _mappings[typeof(TError)] = (statusCode, title, typeUrl);

    }
    public static (int statusCode, string title, string typeUrl)? GetMapping(AppError error)
    {
        var type = error.GetType();
        return _mappings.TryGetValue(type, out var mapping) ? mapping : null;
    }

    public static void RegisterDefaults()
    {
        Register<DuplicatedEmailError>(StatusCodes.Status409Conflict, "Email Duplicated", ConflictUrl);
        Register<UserRoleNotAllowedError>(
            StatusCodes.Status403Forbidden,
            "User Role Not Allowed",
            ForbiddenUrl
        );
    }
}
