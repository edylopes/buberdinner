using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication.Commands.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using OneOf;

namespace BuberDinner.Application.Services.Authentication.Commands;

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

    public async Task<OneOf<AuthenticationResult, AppError>> Login(
        string email,
        string password,
        string? existingRefreshToken = null
    )
    {
        if (await _userRepository.GetByEmailAsync(email) is not User user)
        {
            return new UserNotFoundError();
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return new InvalidCredentialError();

        var (accessToken, refreshToken) = _jwtTokenGenerator.GenerateTokens(user);

        // Adicionar o novo token ao usuário (sem revogar os existentes para permitir múltiplas sessões)
        user.AddRefreshToken(refreshToken);

        await _userRepository.UpdateAsync(user);

        return MapAuthResult(user, accessToken, refreshToken);
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

        var (accessToken, refreshToken) = _jwtTokenGenerator.GenerateTokens(user);

        user.AddRefreshToken(refreshToken);

        await _userRepository.AddAsync(user);

        return MapAuthResult(user, accessToken, refreshToken);
    }

    private static AuthenticationResult MapAuthResult(
        User user,
        string accessToken,
        RefreshToken refreshToken
    )
    {
        return new AuthenticationResult(user, accessToken, refreshToken.Token);
    }
}
