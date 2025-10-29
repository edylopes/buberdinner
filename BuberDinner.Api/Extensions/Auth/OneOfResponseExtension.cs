


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
public static class OneOfResponseExtension
{
    public static IActionResult ToResponseResult<TSuccess>(
        this OneOf<TSuccess, AppError> result,
        Func<TSuccess, IActionResult> onSuccess)
    {
        return result.Match(
            success => onSuccess(success),
            error => ErrorResults.FromError(error)
        );
    }

    public static IActionResult ToAuthResponse(
        this OneOf<AuthenticationResult, AppError> result,
        IMapper mapper,
        Func<AuthResponse, ResponseResult<AuthResponse>> responseFactory
        )
    {
        return result.ToResponseResult(
            success =>
            {
                var payload = mapper.Map<AuthResponse>(success);
                var response = responseFactory(payload);
                return response
                     .WithCookie("RefreshToken", success.RefreshToken)
                     .WithHeader("Authorization", success.AccessToken);
            }
        );

    }
    public static IActionResult ToRegister(
    this OneOf<AuthenticationResult, AppError> result,
    IMapper mapper)
    {
        return result.ToAuthResponse(
            mapper,
            success =>
            {
                var payload = mapper.Map<AuthResponse>(success);
                return HttpResults.Created(payload, $"api/v1/users{payload.Id}");
            });
    }

    public static IActionResult ToLogin(
    this OneOf<AuthenticationResult, AppError> result,
    IMapper mapper)
    {
        return result.ToAuthResponse(
            mapper,
            payload => HttpResults.Ok(payload)
        );
    }
    public static IActionResult ToOk<TSuccess>(this OneOf<TSuccess, AppError> result)
        => result.ToResponseResult(success => HttpResults.Ok(success));
}