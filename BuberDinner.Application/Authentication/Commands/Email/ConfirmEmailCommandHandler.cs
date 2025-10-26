using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using BuberDinner.Application.Common.Dto.Email.Enums;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using Microsoft.Extensions.Configuration;
using System.Text;
using BuberDinner.Application.Common.Interfaces.Authentication;


namespace BuberDinner.Application.Authentication.Commands.Email;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, OneOf<string, EmailConfirmationError>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public ConfirmEmailCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IConfiguration configuration)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<OneOf<string, EmailConfirmationError>> Handle(
        ConfirmEmailCommand command,
        CancellationToken cancellationToken)
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
                return EmailConfirmationError.InvalidToken;

            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                return EmailConfirmationError.UserNotFound;

            if (user.EmailConfirmed)
                return EmailConfirmationError.EmailAlreadyConfirmed;

            user.ConfirmEmail();

            _userRepository.Update(user);
            // await _unitOfWork.CommitAsync(cancellationToken);

            return user.Id.ToString();
        }
        catch (Exception)
        {
            return EmailConfirmationError.InvalidToken; ;
        }

    }
}