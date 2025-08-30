namespace BuberDinner.Infrastructure.Configuration;

public class JwtSettings
{
    public const string JWT = "JWT";
    public string? JwtSecretKey { get; init; }
    public string Audience { get; init; }
    public string? Issuer { get; init; }
    public int ExpireMinutes { get; init; }
}
