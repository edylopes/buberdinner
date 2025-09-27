using BuberDinner.Api.Extensions.Auth;
using BuberDinner.Domain.Common.Errors;
using OneOf;

namespace BuberDinner.Api.Extensions;


//Generic method 
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
}