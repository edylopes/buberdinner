namespace BurberDinner.Infrastructure.Configuration;

public class ConfigurationOptions
{
    public const string JWT = "JWT";
    public string? JwtSecretKey { get;init; }
    public string? Issuer { get; init; }
    public int ExpireMinutes { get; init; }
    public string? Audience { get; init; }
}