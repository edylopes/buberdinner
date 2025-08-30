using BuberDinner.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Results;

public class ResponseResult<T> : IActionResult
{
    private readonly T _payload;
    private bool _isNewResource;
    private int? _statusCode;
    private string? _location;
    private readonly Dictionary<string, string> _cookies = new();
    private readonly Dictionary<string, string> _headers = new();
    private readonly Dictionary<string, CookieOptions?> _pendingCookieOptions = new();

    public ResponseResult(
        T payload,
        bool isNewResource = false,
        int? statusCode = StatusCodes.Status200OK,
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
        // Guarda cookie para ser setado depois
        _cookies[name] = HeaderSanitizer.Sanitizer(value);
        // Opcional: salvar opções personalizadas
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
        new ResponseResult<T>(_payload, false, statusCode: StatusCodes.Status200OK);

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

        if (_payload is null && _statusCode == StatusCodes.Status200OK)
        {
            response.StatusCode = StatusCodes.Status204NoContent;
            var res = new NoContentResult();
            await res.ExecuteResultAsync(context);
            return;
        }

        if (_isNewResource)
        {
            var createdResult = new CreatedResult(_location ?? string.Empty, _payload);
            await createdResult.ExecuteResultAsync(context);
            return;
        }

        var okResult = new ObjectResult(_payload) { StatusCode = _statusCode };
        await okResult.ExecuteResultAsync(context);
    }
}
