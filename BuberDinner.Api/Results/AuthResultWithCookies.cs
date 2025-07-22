using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Filters.Results;

internal class AuthResultWithCookies : IActionResult
{
    public AuthenticationResult Result { get; }

    public AuthResultWithCookies(AuthenticationResult result)
    {
        Result = result;
    }

    public Task ExecuteResultAsync(ActionContext context)
    {
        var Response = context.HttpContext.Response;

        Response.Cookies.Append(
            "refreshToken",
            Result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
            }
        );

        Response.Headers["Authorization"] = $"Bearer {Result.Token}";

        var payload = new AuthenticationResponse(
            Result.Id,
            Result.FirstName,
            Result.LastName,
            Result.Email,
            Result.Role
        );

        var createdResult = new CreatedResult($"user/{Result.Id}", payload);

        return createdResult.ExecuteResultAsync(context);
    }
}
