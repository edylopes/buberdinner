using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities.Users;
using MapsterMapper;
using OneOf;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Authentication.Commands.Register;

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
        LoginQuery req
    )
    {
        if (await _userRepository.GetByEmailAsync(req.email) is not User user)
            return new UserNotFoundError();

        if (!BCrypt.Net.BCrypt.Verify(req.password, user.PasswordHash))
            return new InvalidCredentialError();

        var (accessToken, refreshToken) = _jwtTokenGenerator.GenerateTokens(user);

        await _userRepository.AddRefreshTokenAsync(user.Id, refreshToken);

        return _mapper.Map<AuthenticationResult>(user) with { accessToken = accessToken };

    }

    public async Task<OneOf<AuthenticationResult, AppError>> Register(RegisterCommand req)

    {

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(req.password);
        var user = new User(req.firstName, req.lastName, hashPassword, req.email);
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
