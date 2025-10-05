using BuberDinner.Api.Utils;
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Common.Errors;

public static class ErrorResults
{
    public static IActionResult FromError(AppError error, HttpContext httpContext)
    {
        var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();


        var (statusCode, url, message, title) =
                 ErrorMapper.GetMapping(error) ?? (500, "/error", "Unknown error", null)!;

        var problem = factory.CreateProblemDetails(
            httpContext,
            type: url,
            statusCode: statusCode,
            title: title,
            detail: message
        );

        problem.Extensions["errorType"] = error.GetType().Name;

        return new ObjectResult(problem) { StatusCode = statusCode };
    }
}

