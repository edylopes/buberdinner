using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Results;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries;
using BuberDinner.Contracts.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthenticationController : Controller
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly ISender _mediator;
    private readonly IMapper _mapper;    


    public AuthenticationController(ILogger<AuthenticationController> logger, ISender mediator, IMapper mapper)
    { 
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        var command = _mapper.Map<RegisterCommand>(req);
        
        var result = await _mediator.Send(command);
        
        return result.Match<IActionResult>(
            success =>
            {
                var payload = _mapper.Map<AuthResponse>(success);
                _logger.LogInformation("New user registered: {UserId}", success.User.Id);

                return ResultOkay
                       .Created(payload, $"user/{payload.id}")
                       .WithCookie("RefreshToken", success.RefreshToken)
                       .WithHeader("Authorization", success.AccessToken);
            },
            error =>
            {
                var (statusCode, url, message) = ErrorMapper.ToError(error);
                return Problem(statusCode: statusCode, detail: message, type: url);
            }
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var result = await _mediator.Send(new LoginQuery(req.Email, req.Password));
        return result.Match<IActionResult>(
            success =>
            {
                var payload = _mapper.Map<AuthResponse>(success);
                _logger.LogInformation("User logged in: {UserId}", success.User.Id);

                return ResultOkay
                        .Ok(payload)
                        .WithCookie("RefreshToken", success.RefreshToken)
                        .WithHeader("Authorization", success.AccessToken);
            },
            error =>
            {
                var (statusCode, url, message) = ErrorMapper.ToError(error);
                return Problem(statusCode: statusCode, detail: message, type: url);
            }
        );
    }
}
