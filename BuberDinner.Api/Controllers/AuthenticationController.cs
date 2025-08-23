using BuberDinner.Api.Results;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthenticationController : Controller
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly ISender _mediator;

    public AuthenticationController(ILogger<AuthenticationController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        var result = await _mediator.Send(
            new RegisterCommand(req.FirstName, req.LastName, req.Email, req.Password)
        );

        return result.Match<IActionResult>(
            authResult =>
            {
                var payload = MapAuthResult(authResult);
                _logger.LogInformation("New user registered: {UserId}", authResult.Id);
                return new ResponseResult<AuthResponse>(
                    payload,
                    true,
                    location: $"user/{authResult.Id}"
                )
                    .WithCookie("RefreshToken", authResult.RefreshToken)
                    .WithHeader("Authorization", authResult.Token);
            },
            error => Problem(statusCode: error.StatusCode, detail: error.Message)
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var result = await _mediator.Send(new LoginQuery(req.Email, req.Password));

        return result.Match<IActionResult>(
            success =>
            {
                var payload = MapAuthResult(success);
                _logger.LogInformation("User logged in: {UserId}", success.Id);
                return new ResponseResult<AuthResponse>(payload)
                    .WithCookie("RefreshToken", success.RefreshToken)
                    .WithHeader("Authorization", success.Token);
            },
            error => Problem(statusCode: error.StatusCode, detail: error.Message)
        );
    }

    private static AuthResponse MapAuthResult(AuthenticationResult result) =>
        new AuthResponse(result.Id, result.FirstName, result.LastName, result.Email, result.Role);
}
