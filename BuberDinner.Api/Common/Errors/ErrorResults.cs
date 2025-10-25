using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Common.Errors;

public static class ErrorResults
{
    private static IHttpContextAccessor? _httpContextAccessor;
    private static HttpContext HttpContext => _httpContextAccessor?.HttpContext!;

    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public static IActionResult FromError(AppError error)
    {
        var factory = HttpContext?.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        var (statusCode, url, message, _) = error;

        var problemDetails = factory!.CreateProblemDetails(

            type: url,
            statusCode: statusCode,
            detail: message,
            instance: HttpContext?.Request.Path.Value,
            httpContext: HttpContext!
        );

        problemDetails.Extensions["errorType"] = error.GetType().Name;

        return new ObjectResult(problemDetails) { StatusCode = statusCode, ContentTypes = { "application/problem+json" } };
    }
}
