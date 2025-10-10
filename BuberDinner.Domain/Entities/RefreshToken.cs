
using System.Text.Json.Serialization;
using BuberDinner.Domain.Entities.Users;
namespace BuberDinner.Domain.Entities;


public class RefreshToken
{
    public Guid Id { get; private set; }
    public string Token { get; init; } = string.Empty;
    public DateTimeOffset Expires { get; private set; }

    //public Guid UserId { get; init; } gera erro de concorrência, shadown prop
    public Guid UserId { get; private set; }

    [JsonIgnore]
    public User? User { get; set; } //for navigation 
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
