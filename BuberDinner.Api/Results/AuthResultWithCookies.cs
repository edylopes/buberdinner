using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;


namespace BuberDinner.Api.Results;

internal class AuthResultWithCookies : IActionResult
{
    private readonly AuthenticationResult _result;
    private readonly bool _isNewResource;

    public AuthResultWithCookies(
        AuthenticationResult result,
        bool isNewResource = true
    )
    {
        _result = result;
        _isNewResource = isNewResource;
    }

    public Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.Cookies.Append(
            "refreshToken",
            _result.refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
            }
        );

        response.Headers["Authorization"] = $"Bearer {_result.user}";

        var createdResult = new CreatedResult($"user/{_result.user.Id}", MapAuthResult(_result));
        var OkResult = new OkObjectResult(MapAuthResult(_result));

        return _isNewResource
            ? createdResult.ExecuteResultAsync(context)
            : OkResult.ExecuteResultAsync(context);
    }

    private static AuthResponse MapAuthResult(AuthenticationResult result)
    {
        return new AuthResponse(
            result.user.Id,
            result.user.FirstName,
            result.user.LastName,
            result.user.Email,
            result.user.Role.ToString()
        );
    }
}
