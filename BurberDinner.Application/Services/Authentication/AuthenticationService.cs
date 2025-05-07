using BurberDinner.Application.Common.Interfaces.Authentication;

namespace BurberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public AuthenticationResult Login(string email, string password)
    {
        //Get user from database
        //Check if password is correct
        //check jwt token
        return new AuthenticationResult(Guid.NewGuid(), null, null, email, null, null);
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        // Check if user already is registered
        // (Create User ) generate uniquie ID 
        // Generate JWT Token

        Guid userId = Guid.NewGuid();
        var token = _jwtTokenGenerator
            .GenerateToken(userId, firstName, lastName, email);

        return new AuthenticationResult(userId, firstName, lastName, email, token, password);
    }
}