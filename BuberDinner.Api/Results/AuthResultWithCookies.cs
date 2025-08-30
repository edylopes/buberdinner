using BuberDinner.Api.Controllers;
using BuberDinner.Application.Services.Authentication.Commands.Common;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Results;

internal class AuthResultWithCookies : IActionResult
{
    private readonly AuthenticationResult _result;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly bool _isNewResource;

    public AuthResultWithCookies(
        AuthenticationResult result,
        ILogger<AuthenticationController> logger,
        bool isNewResource = true
    )
    {
        _logger = logger;
        _result = result;
        _isNewResource = isNewResource;
    }

    public Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.Cookies.Append(
            "refreshToken",
            _result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
            }
        );

        response.Headers["Authorization"] = $"Bearer {_result.AccessToken}";

        var createdResult = new CreatedResult($"user/{_result.User.Id}", MapAuthResult(_result));
        var OkResult = new OkObjectResult(MapAuthResult(_result));

        return _isNewResource
            ? createdResult.ExecuteResultAsync(context)
            : OkResult.ExecuteResultAsync(context);
    }

    private static AuthResponse MapAuthResult(AuthenticationResult result)
    {
        return new AuthResponse(
            result.User.Id,
            result.User.FirstName,
            result.User.LastName,
            result.User.Email,
            result.User.Role
        );
    }
}
