using BuberDinner.Application.Errors;
using OneOf;

namespace BuberDinner.Application.Services.Authentication;

public interface IAuthenticationService
{
    public Task<OneOf<AuthenticationResult, AppError>> Login(string email, string password);
    public Task<OneOf<AuthenticationResult, AppError>> Register(
        string firstName,
        string lastName,
        string email,
        string password
    );
}
