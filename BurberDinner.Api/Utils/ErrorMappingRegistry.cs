

using BurberDinner.Application.Errors;

namespace BurberDinner.Api.Utils;

public static class ErrorMappingRegistry
{
    const string conflictUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    const string forbiddenUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
    private static readonly Dictionary<Type, (int statusCode, string title, string typeUrl)> _mappings = new();

    public static void Register<TError>(int statusCode, string title, string typeUrl) //TODO  refactor if necessary
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
        Register<DuplicatedEmailError>(StatusCodes.Status409Conflict, "Email Duplicated", conflictUrl);
        Register<UserRoleNotAllowedError>(StatusCodes.Status403Forbidden, "User Role Not Allowed", forbiddenUrl);
        Register<ValidationError>(StatusCodes.Status400BadRequest, "Invalid email or password.", forbiddenUrl);

    }
}
