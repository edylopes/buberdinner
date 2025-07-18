using BurberDinner.Application.Common.Interfaces.Authentication;
using BurberDinner.Application.Common.Interfaces.Persistence;
using BurberDinner.Domain.Entities;

namespace BurberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<AuthenticationResult> Login(string email, string password)
    {
        // 1. Validate the user exists
        // 2. Validate the password is correct
        if (_userRepository.GetUserByEmail(email) is not { } user)
        {
            return null;
        }

        if (user?.Password != password) throw new Exception("Invalid password.");

        return new AuthenticationResult(user.Id, user.FirstName, user.LastName, user.Email, user.Token);
    }

    public async Task<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        // 1. Validate the user does´t exist
        if (_userRepository?.GetUserByEmail(email) is not null)
        {
            throw new Exception("User with given email already exists");
        }

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };
        // Generate JWT Token

        var token = _jwtTokenGenerator
            .GenerateToken(user);

        _userRepository.Add(user, token);
        user.Token = token;

        return new AuthenticationResult(user.Id, user.FirstName, user.LastName, user.Email, user.Token);
    }
}