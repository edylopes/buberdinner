using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using OneOf;

namespace BuberDinner.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<OneOf<AuthenticationResult, AppError>> Login(
        string email,
        string password,
        string? existingRefreshToken = null
    );
    Task<OneOf<AuthenticationResult, AppError>> Register(
        string firstName,
        string lastName,
        string email,
        string password
    );
}
