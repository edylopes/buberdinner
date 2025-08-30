using BuberDinner.Application.Services.Authentication.Commands.Common;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
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
            success =>
            {
                controller.Response.Cookies.Append(
                    "refreshToken",
                    success.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTimeOffset.UtcNow.AddDays(7),
                    }
                );
                controller.Response.Headers["Authorization"] = $"Bearer {success.AccessToken}";

                return new CreatedResult(
                    $"user/{success.User.Id}",
                    new AuthResponse(
                        success.User.Id,
                        success.User.FirstName,
                        success.User.LastName,
                        success.User.Email,
                        success.User.Role
                    )
                );
            },
            error =>
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Validation Errors",
                    Detail = error.Message,
                    //Status = error.StatusCode,
                };

                return new ObjectResult(problemDetails) { StatusCode = 400 };
            }
        );
    }
}
