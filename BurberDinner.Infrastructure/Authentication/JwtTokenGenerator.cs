using BurberDinner.Application.Common.Interfaces.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BurberDinner.Application.Common.Interfaces.Services;
using BurberDinner.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace BurberDinner.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ConfigurationOptions _options;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<ConfigurationOptions> options)
    {
        _dateTimeProvider = dateTimeProvider;
        _options = options.Value!;
    }

    public string GenerateToken(Guid userId, string firstName, string lastName, string email)
    {
        var handler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, firstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "Admin")

        };

        var creds = new SigningCredentials(key: new SymmetricSecurityKey(key: ConvertSecretToBytes(
            _options.JwtSecretKey!)), algorithm: SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            notBefore: _dateTimeProvider.UtcNow.AddSeconds(5),
            expires: _dateTimeProvider.UtcNow.AddMinutes(_options.ExpireMinutes),
            claims: claims,
            signingCredentials: creds
        );

        return handler.WriteToken(token);
    }

    private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32 = false) 
        => Encoding.UTF8.GetBytes(secret);
}