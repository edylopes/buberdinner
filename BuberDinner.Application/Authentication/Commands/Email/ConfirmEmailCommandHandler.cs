using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using Microsoft.Extensions.Configuration;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Application.Common.Dto;
using BuberDinner.Application.Common.Errors;
using System.Text;


namespace BuberDinner.Application.Authentication.Commands.Email;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, OneOf<EmailConfirmed, AppError>>
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public ConfirmEmailCommandHandler(
    IUserRepository userRepository,
    IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<OneOf<EmailConfirmed, AppError>> Handle(ConfirmEmailCommand command, CancellationToken ct)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretkey = _configuration.GetSection("JwtSettings")["SecretKey"]!;
        try

        {
            var principal = tokenHandler.ValidateToken(command.Token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey!))
            }, out var validatedToken);


            string userId = principal.FindFirst("userId")!.Value;

            if (!Guid.TryParse(userId, out var id))
            {
                return new InvalidToken();
            }

            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                return new EmailConfirmationFailed();

            if (user.EmailConfirmed)
                return new EmailAlreadyConfirmed();

            user.ConfirmEmail();

            _userRepository.Update(user);

            return new EmailConfirmed(email: user.Email);
        }
        catch (Exception)
        {
            throw;
        }

    }
}