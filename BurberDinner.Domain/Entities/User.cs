using System.Net.Mail;
using BurberDinner.Domain.ValueObjects;


namespace BurberDinner.Domain.Entities;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    private UserRole _role { get; set; }
    public string Role
    {
        get => _role.ToString();
        private set => _role = UserRole.Create(value);
    }
    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<RefreshToken>? RefreshTokens => _refreshTokens.AsReadOnly();
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool EmailConfirmed { get; private set; }

    protected User() { }
    public User(string firstName, string lastName, string hash, string email, string? role)
    {

        var address = new MailAddress(email);

        if (address.Address != email)
            throw new ArgumentException("Email inválido");

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = hash;
        Email = email;
        Role = role ?? RoleType.User.ToString();
        EmailConfirmed = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        if (refreshToken == null)
            throw new ArgumentNullException(nameof(refreshToken));

        if (_refreshTokens.Count(rt => !rt.IsExpired || !rt.Revoked) >= 5)
        {
            var oldestToken = _refreshTokens.OrderBy(rt => rt.Created).First();
            _refreshTokens.Remove(oldestToken);

            throw new InvalidOperationException("Refresh tokens limit reached");
        }

        _refreshTokens.Add(refreshToken);
    }


    public void RevokeRefreshToken(Guid refreshTokenId)
    {
        var refreshToken = _refreshTokens.FirstOrDefault(rt => rt.Id == refreshTokenId);
        if (refreshToken == null)
            throw new InvalidOperationException("Refresh token not found");

        refreshToken.Revoke();
    }

    public void ConfirmEmail() => EmailConfirmed = true;

    public void UpdateProfile(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("FirstName is Required");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("LastName is Required");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is Required");

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

    public void ChangeRole(string newRole)
    {
        Role = newRole;
        UpdatedAt = DateTime.UtcNow;
    }

}