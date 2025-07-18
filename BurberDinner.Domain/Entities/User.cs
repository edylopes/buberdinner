namespace BurberDinner.Domain.Entities;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    private readonly List<RefreshToken> _refreshTokens = new();

    public IReadOnlyCollection<RefreshToken>? RefreshTokens => _refreshTokens.AsReadOnly();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool EmailConfirmed { get; private set; }


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

}