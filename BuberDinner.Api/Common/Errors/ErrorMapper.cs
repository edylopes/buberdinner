using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Common.Errors;

internal static class ErrorMapper
{
    private static readonly Dictionary<Type, AppError> Mappings = new();
    public static void Register(AppError error)

    {
        var type = error.GetType();
        Mappings[type] = error;

    }

    public static AppError? GetMapping(AppError error)
    {
        Register(error);
        var type = error.GetType();
        return Mappings.TryGetValue(type, out var mapping) ? mapping : null;
    }

}
