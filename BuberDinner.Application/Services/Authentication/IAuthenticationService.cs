
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Commands.Login;
using BuberDinner.Domain.Common.Errors;
using OneOf;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Application.Common.Errors;

namespace BuberDinner.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<OneOf<AuthenticationResult, AppError>> Login(LoginCommand req);
    Task<OneOf<AuthenticationResult, AppError>> Register(
        RegisterCommand request
    );
    Task<OneOf<EmailConfimed, AppError>> ConfirmEmailAsync(Guid userId, CancellationToken ct);
}
