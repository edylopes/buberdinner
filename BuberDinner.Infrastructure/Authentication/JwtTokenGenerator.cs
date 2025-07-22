using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BuberDinner.Domain.Entities;
using BuberDinner.Infrastructure.Configuration;
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

    public string GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Iat, _dateTimeProvider.UtcNow.ToShortDateString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var creds = new SigningCredentials(
            key: new SymmetricSecurityKey(ConvertSecretToBytes(_options.JwtSecretKey!)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            notBefore: _dateTimeProvider.UtcNow.AddSeconds(5),
            expires: _dateTimeProvider.UtcNow.AddMinutes(15),
            claims: claims,
            signingCredentials: creds
        );

        return handler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(User user)
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
