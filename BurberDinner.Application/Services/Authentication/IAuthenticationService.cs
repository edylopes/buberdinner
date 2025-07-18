namespace BurberDinner.Application.Services.Authentication;

public interface IAuthenticationService
{
    public Task<AuthenticationResult> Login(string email, string password);
    public Task<AuthenticationResult> Register(string firstName, string lastName, string email, string password);
}
