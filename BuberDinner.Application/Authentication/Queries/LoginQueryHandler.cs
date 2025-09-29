using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;
using BuberDinner.Contracts.Authentication;
using MapsterMapper;

namespace BuberDinner.Application.Authentication.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, OneOf<AuthenticationResult, AppError>>
{
    private readonly IAuthenticationService _authService;
    private readonly IMapper _mapper;

    public LoginQueryHandler(IAuthenticationService authenticationService, IMapper mapper)
    {
        _authService = authenticationService;
        _mapper = mapper;
    }
    public Task<OneOf<AuthenticationResult, AppError>> Handle(LoginQuery command, CancellationToken cxt)
    {
        var result = _authService.Login(_mapper.Map<LoginRequest>(command));
        return result;
    }
}
