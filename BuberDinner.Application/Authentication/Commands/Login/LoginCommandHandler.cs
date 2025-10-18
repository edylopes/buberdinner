using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, OneOf<AuthenticationResult, AppError>>
{
    private readonly IAuthenticationService _authService;

    public LoginCommandHandler(IAuthenticationService authService)
    {
        _authService = authService;
    }
    public async Task<OneOf<AuthenticationResult, AppError>> Handle(LoginCommand query, CancellationToken cxt)
    {
        var result = await _authService.Login(query);
        return result;
    }
}
