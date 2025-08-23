using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, OneOf<AuthenticationResult, AppError>>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginQueryHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public Task<OneOf<AuthenticationResult, AppError>> Handle(
        LoginQuery command,
        CancellationToken cancellationToken
    )
    {
        var result = _authenticationService.Login(command.Email, command.Password);
        return result;
    }
}
