using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;
using BuberDinner.Contracts.Authentication;
using MapsterMapper;
using Mapster;

namespace BuberDinner.Application.Authentication.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, OneOf<AuthenticationResult, AppError>>
{
    private readonly IAuthenticationService _authService;

    public LoginQueryHandler(IAuthenticationService authenticationService, IMapper mapper)
    {
        _authService = authenticationService;
    }
    public async Task<OneOf<AuthenticationResult, AppError>> Handle(LoginQuery query, CancellationToken cxt)
    {
        var result = await _authService.Login(query);
        return result;
    }
}
