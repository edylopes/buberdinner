using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, OneOf<AuthenticationResult, AppError>>
{
    private readonly IAuthenticationService _authenticationService;

    public RegisterCommandHandler(IAuthenticationService authtenticarionService)
    {
        _authenticationService = authtenticarionService;
    }

    public async Task<OneOf<AuthenticationResult, AppError>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await _authenticationService.Register(
            command.FirstName,
            command.LastName,
            command.Email,
            command.Password
        );

        return result;
    }
}
