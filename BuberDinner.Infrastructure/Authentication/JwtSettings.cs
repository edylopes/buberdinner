namespace BuberDinner.Infrastructure.Configuration;

public class JwtSettings
{
    public const string JWT = "JwtSettings";
    public string? SecretKey { get; init; }
    public string Audience { get; init; }
    public string? Issuer { get; init; }
    public int ExpireMinutes { get; init; }
}
