using BuberDinner.Api.Results;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace BuberDinner.Api.Extensions.Auth;

public static class AuthResponses
{
    public static IActionResult ToAuthResponse(
        this OneOf<AuthenticationResult, AppError> result,
        IMapper mapper,
        HttpContext httpContext)
    {
        return result.ToResponseResult(
            success =>
            {
                var payload = mapper.Map<AuthResponse>(success);
                return HttpResults.Created(payload, $"user/{payload.id}")
                    .WithCookie("RefreshToken", success.refreshToken)
                    .WithHeader("Authorization", success.accessToken);
            },
            httpContext
        );
    }
}