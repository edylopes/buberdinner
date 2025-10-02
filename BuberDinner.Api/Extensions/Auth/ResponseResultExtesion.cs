

using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Results;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using MapsterMapper;
using OneOf;

namespace BuberDinner.Api.Extensions.Auth;

/// <summary>
/// Extension for converting OneOf results to IActionResult
/// </summary>

//Generic extension for OneOf<TSuccess, TError>
public static class OneOfResponseExtension
{
    public static IActionResult ToResponseResult<TSuccess>(
        this OneOf<TSuccess, AppError> result,
        Func<TSuccess, IActionResult> onSuccess,
        HttpContext httpContext)
    {
        return result.Match(
            success => onSuccess(success),
            error => ErrorResults.FromError(error, httpContext)
        );
    }

    public static IActionResult ToAuthResponse(
        this OneOf<AuthenticationResult, AppError> result,
        IMapper mapper,
        HttpContext httpContext)
    {
        return result.ToResponseResult(
            success =>
            {
                var payload = mapper.Map<AuthResponse>(success);

                return HttpResults.Created(payload, $"/api/v1/user/{payload.id}")
                            .WithCookie("RefreshToken", success.refreshToken)
                            .WithHeader("Authorization", success.accessToken);


            },
            httpContext
        );
    }
}