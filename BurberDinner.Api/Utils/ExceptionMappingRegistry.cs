

using BurberDinner.Domain.DomainErrors;

namespace BurberDinner.Api.Utils;

public static class ExceptionMappingRegistry
{
    const string conflictUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    const string forbiddenUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
    private static readonly Dictionary<Type, (int statusCode, string title, string typeUrl)> _mappings = new();

    public static void Register<TEx>(int statusCode, string title, string typeUrl)
        where TEx : AppError
    {
        _mappings[typeof(TEx)] = (statusCode, title, typeUrl);
    }

    public static (int statusCode, string title, string typeUrl)? GetMapping(object error)
    {
        var type = error.GetType();
        return _mappings.TryGetValue(type, out var mapping) ? mapping : null;
    }

    public static void RegisterDefaults()
    {
        Register<DuplicatedEmailError>(StatusCodes.Status409Conflict, "Email Duplicated", conflictUrl);
        Register<UserRoleNotAllowedError>(StatusCodes.Status403Forbidden, "User Role Not Allowed", forbiddenUrl);
    }
}
