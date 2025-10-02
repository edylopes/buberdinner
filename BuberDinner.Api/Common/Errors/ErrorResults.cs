using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Common.Errors;

public static class ErrorResults
{
    public static IActionResult FromError(AppError error, HttpContext httpContext)
    {
        var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        var (statusCode, url, message, title) = ErrorMapper.ToError(error);

        var problem = factory.CreateProblemDetails(
            httpContext,
            statusCode: statusCode,
            title: title ?? null,
            detail: message,
            type: url
        );

        problem.Extensions["errorType"] = error.GetType().Name;

        return new ObjectResult(problem) { StatusCode = statusCode };
    }
}

