using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using MapsterMapper;
using OneOf;
using BuberDinner.Contracts.Authentication;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AuthenticationService(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        IMapper mapper
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<OneOf<AuthenticationResult, AppError>> Login(
        LoginRequest req
    )
    {
        if (await _userRepository.GetByEmailAsync(req.Email) is not User user)
        {
            return new UserNotFoundError();
        }

        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return new InvalidCredentialError();

        var (accessToken, refreshToken) = _jwtTokenGenerator.GenerateTokens(user);

        // Adicionar o novo token ao usuário (sem revogar os existentes para permitir múltiplas sessões)
        user.AddRefreshToken(refreshToken);


        await _userRepository.UpdateAsync(user);

        var result = _mapper.Map<AuthenticationResult>(user) with { accessToken = accessToken };

        return result;
    }

    public async Task<OneOf<AuthenticationResult, AppError>> Register(RegisterRequest req)

    {

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(req.Password);
        var user = new User(req.FirstName, req.LastName, hashPassword, req.Email);
        // user.AddRole(UserRole.Create(RoleType.Admin.ToString()));
        try
        {

            var (token, refreshToken) = _jwtTokenGenerator.GenerateTokens(user);

            user.AddRefreshToken(refreshToken);

            await _userRepository.AddAsync(user);

            return _mapper.Map<AuthenticationResult>(user) with { accessToken = token };
        }
        catch (Exception ex) when (ex is InvalidOperationException && ex.Message.Contains("email"))
        {
            return new DuplicatedEmailError();
        }

    }
}
