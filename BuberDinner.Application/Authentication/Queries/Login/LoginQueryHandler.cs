using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Common.Interfaces.Authentication;
using MapsterMapper;

namespace BuberDinner.Application.Authentication.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, OneOf<AuthenticationResult, AppError>>
{
    private readonly IAuthenticationService _authService;

    public LoginQueryHandler(IAuthenticationService authService)
    {
        _authService = authService;
    }
    public async Task<OneOf<AuthenticationResult, AppError>> Handle(LoginQuery query, CancellationToken cxt)
    {
        var result = await _authService.Login(query);
        return result;
    }
}
