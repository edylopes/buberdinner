namespace BurberDinner.Infrastructure.Configuration;

public class JwtSettings
{
    public const string JWT = "JWT";
    public string? JwtSecretKey { get; init; }
    public string? Issuer { get; init; } = "BurberDinner";
    public int ExpireMinutes { get; init; }
    public string? Audience { get; init; } = "BurberDinner";
}