using BuberDinner.Application.Errors;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace BuberDinner.Api.Extensions;

public static class OneOfExtensions
{
    public static IActionResult ToAuthActionResult(
        this OneOf<AuthenticationResult, AppError> result,
        ControllerBase controller
    )
    {
        return result.Match<IActionResult>(
            user =>
            {
                controller.Response.Cookies.Append(
                    "refreshToken",
                    user.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                    }
                );
                controller.Response.Headers["Authorization"] = $"Bearer {user.Token}";

                return new CreatedResult(
                    $"user/{user.Id}",
                    new AuthenticationResponse(
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.Role
                    )
                );
            },
            error =>
            {
                var problemDetails = new ProblemDetails
                {
                    Type = error.TypeUrl,
                    Title = "Validation Errors",
                    Detail = error.Message,
                    Status = error.StatusCode,
                };

                return new ObjectResult(problemDetails) { StatusCode = error.statusCode };
            }
        );
    }
}
