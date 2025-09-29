using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using OneOf;
using MapsterMapper;
using BuberDinner.Contracts.Authentication;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, OneOf<AuthenticationResult, AppError>>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(IAuthenticationService authenticationService, IMapper mapper)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
    }

    public async Task<OneOf<AuthenticationResult, AppError>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken
    )
    {
        var request = _mapper.Map<RegisterRequest>(command);
        return await _authenticationService.Register(request);


    }
}
