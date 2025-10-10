using System.Net.Mail;
using BuberDinner.Domain.Events;
using BuberDinner.Domain.Events.Interfaces;
using BuberDinner.Domain.Exceptions;
using BuberDinner.Domain.ValueObjects;

namespace BuberDinner.Domain.Entities.Users;

public class User : IHasDomainEvents
{
    public Guid Id { get; private set; }
    public List<UserRole> Roles { get; private set; } = new();

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool EmailConfirmed { get; private set; }

    public IReadOnlyList<IDomainEvent> domainEvents => throw new NotImplementedException();

    // For EF Core
    protected User() { }

    public User(string firstName, string lastName, string passwordHash, string email)
    {
        var address = new MailAddress(email);

        if (address.Address != email)
            throw new ArgumentException("Invalid email");

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
        Email = email;
        Roles = new List<UserRole> { UserRole.Create(nameof(RoleType.User)) };
        EmailConfirmed = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        if (refreshToken == null)
            throw new ArgumentNullException();

        if (_refreshTokens.Any(r => r.Token.Trim() == refreshToken.Token.Trim()))
            throw new InvalidOperationException("RefreshToken already exist"); ;

        if (_refreshTokens.Count(rt => rt is { IsExpired: false, Revoked: false }) >= 5)
            throw new RefreshTokenLimitExceededException("Refresh token quota reached.");

        if (!_refreshTokens.Any(t => t.Id == refreshToken.Id))
            _refreshTokens.Add(refreshToken);
    }

    public void RevokeRefreshToken(Guid refreshTokenId)
    {
        var refreshToken = _refreshTokens.FirstOrDefault(rt => rt.Id == refreshTokenId);
        if (refreshToken == null)
            throw new ArgumentException("Refresh token not found");

        refreshToken.Revoke();
    }

    public void ConfirmEmail() => EmailConfirmed = true;

    public void UpdateProfile(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("FirstName is Required");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("LastName is Required");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is Required");

        var address = new MailAddress(email);

        if (address.Address != email)
            throw new ArgumentException("Email inválido");

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("Password cannot be empty");

        PasswordHash = newPassword;
        UpdatedAt = DateTime.UtcNow;
    }
    public static User CreateAdmin(string firstName, string lastName, string passwordHash, string email)
         => new User(firstName, lastName, passwordHash, email) { Roles = new List<UserRole> { UserRole.Create(nameof(RoleType.Admin)) } };
    public void AddRole(UserRole newRole)
    {
        Roles.Add(newRole);
        UpdatedAt = DateTime.UtcNow;
    }

    //Domain Event
    public void Login()
    {
        AddDomainEvent(new UserLoggedInEvent(this.Id));
    }
    public void ClearDomainEvents() => _domainEvents.Clear();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

}
