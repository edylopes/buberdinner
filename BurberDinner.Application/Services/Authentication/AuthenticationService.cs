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

    public AuthenticationResult Login(string email, string password)
    {
        // 1. Validate the user  exist.
        if (_userRepository.GetUserByEmail(email) is not { } user) //User
        {
            throw new Exception("Email or Password  is not correct.");
        }
        //Check if password is correct.
        if (user.Password != password) throw new Exception("Invalid password.");
        //create jwt token
        var token = _jwtTokenGenerator
            .GenerateToken(user);
        return new AuthenticationResult(user, token);
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        // 1. Validate the user doesn´t exist
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            throw new Exception("The user already exists");
        }

        // (Create User ) generate uniquie ID & persiste to DB
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
        };
        _userRepository.Add(user);
        // Generate JWT Token
        var token = _jwtTokenGenerator
            .GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}