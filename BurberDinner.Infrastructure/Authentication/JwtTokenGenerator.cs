using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BurberDinner.Domain.Entities;
using Microsoft.Extensions.Options;
using BurberDinner.Infrastructure.Configuration;



namespace BurberDinner.Infrastructure.Authentication;

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
        };
        var creds = new SigningCredentials(key: new SymmetricSecurityKey(ConvertSecretToBytes(
            _options.JwtSecretKey!)), SecurityAlgorithms.HmacSha256);

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

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32 = false)
        => Encoding.UTF8.GetBytes(secret);
}