using BuberDinner.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Results;

public class ResponseResult<T> : IActionResult
{

    private readonly T _payload;
    private readonly bool _isNewResource;
    private readonly int _statusCode;
    private readonly string? _location;
    private readonly Dictionary<string, string> _cookies = new();
    private readonly Dictionary<string, string> _headers = new();
    private readonly Dictionary<string, CookieOptions?> _pendingCookieOptions = new();

    public ResponseResult(T payload, bool isNewResource = false, int statusCode = StatusCodes.Status200OK, string? location = null)
    {
        _payload = payload;
        _isNewResource = isNewResource;
        _location = location;
        _statusCode = statusCode;
    }


    public ResponseResult<T> WithCookie(
     string name,
     string value,
     CookieOptions? options = null)
    {
        // Guarda cookie para ser setado depois
        _cookies[name] = HeaderSanitizer.Sanitizer(value);

        // Opcional: salvar opções personalizadas 
        _pendingCookieOptions[name] = options ?? new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };

        return this;
    }

    public ResponseResult<T> WithHeader(string name, string value)
    {
        _headers[name] = HeaderSanitizer.Sanitizer(value);
        return this;
    }

    public Task ExecuteResultAsync(ActionContext context)
    {

        var response = context.HttpContext.Response;

        foreach (var kvp in _cookies)
        {

            response.Cookies.Append(
                kvp.Key,
                kvp.Value,
                _pendingCookieOptions[kvp.Key]!
                );

        }

        foreach (var kvp in _headers)
        {
            response.Headers[kvp.Key] = kvp.Value;
        }


        if (_isNewResource)
        {
            var createdResult = new CreatedResult(_location ?? string.Empty, _payload);
            return createdResult.ExecuteResultAsync(context);
        }

        var okResult = new ObjectResult(_payload) { StatusCode = _statusCode };
        return okResult.ExecuteResultAsync(context);


    }
}
