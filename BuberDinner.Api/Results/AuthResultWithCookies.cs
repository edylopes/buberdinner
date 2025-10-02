using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using MapsterMapper;


namespace BuberDinner.Api.Results;

internal class AuthResultWithCookies : IActionResult
{
    private readonly AuthenticationResult _result;
    private readonly bool _isNewResource;

    private readonly IMapper _mapper;

    public AuthResultWithCookies(
        AuthenticationResult result,
        IMapper mapper,
        bool isNewResource = true
        )
    {
        _result = result;
        _isNewResource = isNewResource;
        _mapper = mapper;
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

        var result = _mapper.Map<AuthResponse>(_result);

        var createdResult = new CreatedResult($"user/{result.id}", result);

        var OkResult = new OkObjectResult(result);

        return _isNewResource
            ? createdResult.ExecuteResultAsync(context)
            : OkResult.ExecuteResultAsync(context);
    }
}
