using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using MapsterMapper;
using OneOf;
using BuberDinner.Application.Authentication.Commands.Login;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using OneOf.Types;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Application.Common.Errors;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    public AuthenticationService(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        IMapper mapper,
        IUnitOfWork uow)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _mapper = mapper;
        _uow = uow;
    }


    public async Task<OneOf<AuthenticationResult, AppError>> Login(
        LoginCommand req
    )
    {
        //Error First 
        if (await _userRepository.GetByEmailAsync(req.email) is not User user)
            return new UserNotFoundError();

        if (!user.EmailConfirmed)
            return new EmailNotConfirmedError();

        if (!BCrypt.Net.BCrypt.Verify(req.password, user.PasswordHash))
            return new InvalidCredentialError();

        var (accessToken, refreshToken) = _jwtTokenGenerator.GenerateTokens(user);

        user.AddRefreshToken(refreshToken);

        _userRepository.MarkAsAdded(refreshToken);
        return _mapper.Map<AuthenticationResult>(user) with { accessToken = accessToken };

    }
    public async Task<OneOf<AuthenticationResult, AppError>> Register(RegisterCommand req)

    {

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(req.password);
        var user = new User(req.firstName, req.lastName, hashPassword, req.email);

        if (await _userRepository.ExistsAsync(u => u.Email == req.email))
            return new DuplicatedEmailError();

        var (token, refreshToken) = _jwtTokenGenerator.GenerateTokens(user);

        user.AddRefreshToken(refreshToken);

        await _userRepository.AddAsync(user);

        return _mapper.Map<AuthenticationResult>(user) with { accessToken = token };

    }
}

