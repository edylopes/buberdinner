using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Errors;
using BuberDinner.Domain.Entities;
using OneOf;

namespace BuberDinner.Application.Services.Authentication;

internal class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<OneOf<AuthenticationResult, AppError>> Login(string email, string password)
    {
        if (await _userRepository.GetByEmailAsync(email) is not User user)
        {
            return new UserNotFoundError();
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return new InvalidCredentialError();

        var accessToken = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            accessToken,
            user.RefreshTokens?.FirstOrDefault(t => t.UserId == user.Id)?.Token ?? string.Empty,
            user.Role
        );
    }

    public async Task<OneOf<AuthenticationResult, AppError>> Register(
        string firstName,
        string lastName,
        string email,
        string password
    )
    {
        if (await _userRepository.GetByEmailAsync(email) is not null)
        {
            return new DuplicatedEmailError();
        }
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User(firstName, lastName, hashPassword, email);

        var accessToken = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);

        user.AddRefreshToken(refreshToken);

        await _userRepository.AddAsync(user);

        return new AuthenticationResult(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            accessToken,
            refreshToken.Token,
            user.Role
        );
    }
}
