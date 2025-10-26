using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MapsterMapper;
using BuberDinner.Application.Authentication.Commands.Login;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Domain.Entities.Users;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public AuthenticationService(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _mapper = mapper;
    }


    public async Task<OneOf<AuthenticationResult, AppError>> Login(
        LoginCommand req
    )
    {
        //Error First 
        if (await _userRepository.GetByEmailAsync(req.Email) is not User user)
            return new UserNotFoundError();

        if (!user.EmailConfirmed)
            return new EmailNotConfirmedError();

        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return new InvalidCredentialError();

        var (accessToken, refreshToken) = _jwtTokenGenerator.GenerateTokens(user);

        user.AddRefreshToken(refreshToken);

        _userRepository.MarkAsAdded(refreshToken);
        return _mapper.Map<AuthenticationResult>(user) with { AccessToken = accessToken };

    }
    public async Task<OneOf<AuthenticationResult, AppError>> Register(RegisterCommand req)

    {

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(req.Password);
        var user = new User(req.FirstName, req.LastName, hashPassword, req.Email);

        if (await _userRepository.ExistsAsync(u => u.Email == req.Email))
            return new DuplicatedEmailError();

        var (token, refreshToken) = _jwtTokenGenerator.GenerateTokens(user);

        user.AddRefreshToken(refreshToken);

        await _userRepository.AddAsync(user);

        return _mapper.Map<AuthenticationResult>(user) with { AccessToken = token };

    }
}

