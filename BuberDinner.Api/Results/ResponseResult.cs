

using BuberDinner.Api.Helpers;

namespace BuberDinner.Api.Results;

public class ResponseResult<T> : IActionResult
{
    private readonly T _payload;
    private readonly bool _isNewResource;
    private readonly int? _statusCode;
    private readonly string? _location;
    private readonly Dictionary<string, string> _cookies = new();
    private readonly Dictionary<string, string> _headers = new();
    private readonly Dictionary<string, CookieOptions?> _pendingCookieOptions = new();

    public ResponseResult(
         T payload,
         bool isNewResource = false,
         int statusCode = StatusCodes.Status200OK,
         string? location = null
     )
    {
        _payload = payload;
        _isNewResource = isNewResource;
        _statusCode = statusCode;
        _location = location;
    }

    public ResponseResult<T> WithCookie(string name, string value, CookieOptions? options = null)
    {
        // Save cookie to be added later
        _cookies[name] = HeaderSanitizer.Sanitizer(value);

        // Opcional: save cookies personalized
        _pendingCookieOptions[name] =
            options
            ?? new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
            };

        return this;
    }

    public ResponseResult<T> WithHeader(string name, string value)
    {
        _headers[name] = HeaderSanitizer.Sanitizer(value);
        return this;
    }

    public ResponseResult<T> AsCreated(string location) =>
        new(_payload, true, StatusCodes.Status201Created, location);

    public ResponseResult<T> AsOk() =>
        new(_payload, false, StatusCodes.Status200OK);

    public ResponseResult<T> AsNoContent() =>
        new(default!, false, statusCode: StatusCodes.Status204NoContent);

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        foreach (var kvp in _cookies)
        {
            response.Cookies.Append(
                kvp.Key,
                kvp.Value,
                _pendingCookieOptions[kvp.Key]!);
        }

        foreach (var kvp in _headers)
        {
            response.Headers[kvp.Key] = kvp.Value;
        }


        var result = CreateHttpResult(context);
        await result.ExecuteResultAsync(context);
    }

    private IActionResult CreateHttpResult(ActionContext context)
    {
        if (_statusCode == 204 || _payload is null)
            return new StatusCodeResult(StatusCodes.Status204NoContent);

        if (_isNewResource)
            return new CreatedResult(_location ?? string.Empty, _payload);

        return new ObjectResult(_payload) { StatusCode = _statusCode };
    }
}