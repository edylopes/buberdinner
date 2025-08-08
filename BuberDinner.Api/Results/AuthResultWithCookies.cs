using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Results;

internal class AuthResultWithCookies : IActionResult
{
    private readonly AuthenticationResult _result;

    public AuthResultWithCookies(AuthenticationResult result)
    {
        _result = result;
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
        response.Headers["Authorization"] = $"Bearer {_result.Token}";

        var createdResult = new CreatedResult($"user/{_result.Id}", MapAuthResult(_result));

        return createdResult.ExecuteResultAsync(context);
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult result)
    {
        return new AuthenticationResponse(
            result.Id,
            result.FirstName,
            result.LastName,
            result.Email,
            result.Role
        );
    }
}
