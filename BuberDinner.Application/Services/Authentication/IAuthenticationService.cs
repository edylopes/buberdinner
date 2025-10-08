
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Domain.Common.Errors;
using OneOf;

namespace BuberDinner.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<OneOf<AuthenticationResult, AppError>> Login(LoginQuery req);
    Task<OneOf<AuthenticationResult, AppError>> Register(
        RegisterCommand request
    );
}
