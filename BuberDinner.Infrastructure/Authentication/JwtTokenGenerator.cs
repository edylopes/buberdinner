using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BuberDinner.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BuberDinner.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _options;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> options)
    {
        _dateTimeProvider = dateTimeProvider;
        _options = options.Value!;
    }

    public (string Token, RefreshToken refreshToken) GenerateTokens(User user)
    {
        var token = GenerateToken(user);
        var refreshToken = GenerateRefreshToken(user);

        return (token, refreshToken);
    }

    private string GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        List<Claim> claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.Name, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Iat,
                    EpochTime.GetIntDate(_dateTimeProvider.UtcNow).ToString(),
                    ClaimValueTypes.Integer64 ),

             };

        foreach (var role in user.Roles)
            claims.Add(new(ClaimTypes.Role, role.ToString()));






        var creds = new SigningCredentials(
            key: new SymmetricSecurityKey(ConvertSecretToBytes(_options.SecretKey!)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            notBefore: _dateTimeProvider.UtcNow.AddSeconds(3),
            expires: _dateTimeProvider.UtcNow.AddMinutes(30),
            claims: claims,
            signingCredentials: creds
        );

        return handler.WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(User user)
    {
        byte[] randomBytes = GenerateRandomBytes();

        return new RefreshToken(
            token: Convert.ToBase64String(randomBytes),
            expires: _dateTimeProvider.UtcNow.AddDays(7),
            userId: user.Id
        );
    }

    private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32 = false) =>
        Encoding.UTF8.GetBytes(secret);

    private static byte[] GenerateRandomBytes(byte length = 64)
    {
        if (length > 255)
            throw new ArgumentOutOfRangeException(
                nameof(length),
                "Length must be less than or equal to 255."
            );

        var randomBytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return randomBytes;
    }
}
