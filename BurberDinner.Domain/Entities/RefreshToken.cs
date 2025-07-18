using System.Text.Json.Serialization;

namespace BurberDinner.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public string Token { get; private set; } = string.Empty;
    private DateTime Expires { get; }
    public Guid UserId { get; private set; }
    public DateTime Created { get; private set; }
    [JsonIgnore]
    public User User { get; set; } = null!;
    public DateTime? RevokedAt { get; private set; }

    public bool IsActive => !Revoked && !IsExpired;
    public bool Revoked { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;

    public RefreshToken(string token, DateTime expires, Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Token = token;
        Expires = expires;
        Created = DateTime.UtcNow;
    }

    public void Revoke()
    {
        Revoked = true;
        RevokedAt = DateTime.UtcNow;
    }
}