using System.Text.Json.Serialization;

namespace BuberDinner.Domain.Entities.Users;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public string Token { get; init; } = string.Empty;
    public DateTimeOffset Expires { get; private set; }
    public Guid UserId { get; init; }
    public User User { get; set; }
    public DateTime Created { get; private set; }

    protected RefreshToken() { }
    public DateTime? RevokedAt { get; private set; }

    public bool IsActive => !Revoked && !IsExpired;
    public bool Revoked { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;

    public RefreshToken(string token, DateTimeOffset expires, Guid userId)
    {
        Id = Guid.NewGuid();
        Token = token;
        Expires = expires;
        Created = DateTime.UtcNow;
        UserId = userId;
    }

    public void Revoke()
    {
        Revoked = true;
        RevokedAt = DateTime.UtcNow;
    }
}
