using BuberDinner.Api.Extensions;
using BuberDinner.Api.Helpers;
using BuberDinner.Api.Results;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthenticationController : Controller
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthenticationService _authService;

    public AuthenticationController(
        IAuthenticationService authenticationService,
        ILogger<AuthenticationController> logger
    )
    {
        _authService = authenticationService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        var result = await _authService.Register(
            req.FirstName,
            req.LastName,
            req.Email,
            req.Password
        );

        return result.Match<IActionResult>(
            authResult =>
            {
                var payload = MapAuthResult(authResult);
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
        var result = await _authService.Login(req.Email, req.Password);

        return result.Match<IActionResult>(
            success =>
            {
                var payload = MapAuthResult(success);
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
