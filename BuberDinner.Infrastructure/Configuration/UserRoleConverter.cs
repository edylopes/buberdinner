using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using BuberDinner.Domain.ValueObjects;

public static class UserRoleConverter
{
    public static readonly ValueConverter<List<UserRole>, string> Converter =
        new ValueConverter<List<UserRole>, string>(
            roles => JsonSerializer.Serialize(roles.Select(r => r.ToString()).ToList(), (JsonSerializerOptions)null!),
            json => ConvertJsonToRoles(json)
        );
    private static List<UserRole> ConvertJsonToRoles(string json)
    {
        var roles = JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions)null!);
        return roles?.Select(v => UserRole.Create(v)).ToList() ?? new List<UserRole>();
    }
}
