using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Common.Errors;

internal static class ErrorMapper
{
    private static readonly Dictionary<Type, AppError> _mappings = new();
    public static void Register(AppError error)

    {
        var type = error.GetType();
        _mappings[type] = error;

    }

    public static AppError? GetMapping(AppError error)
    {
        Register(error);
        var type = error.GetType();
        return _mappings.TryGetValue(type, out var mapping) ? mapping : null;
    }

}
