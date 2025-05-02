namespace BurberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    public AuthenticationResult Login(string email, string password)
    {
        return new AuthenticationResult(Guid.NewGuid(), "Edy", "Lopes", email, password, "125454-25454");
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        return new AuthenticationResult(Guid.NewGuid(), firstName, lastName, email, password, "125454-25454");
    }
}