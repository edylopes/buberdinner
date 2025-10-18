using BuberDinner.Api.Utils;
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Common.Errors;

public static class ErrorResults
{
    public static IActionResult FromError(AppError error)
    {
        // var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        var (statusCode, url, message, title) = error;

        var problem = new ProblemDetails
        {

            Type = url,
            Status = statusCode,
            Title = title,
            Detail = message
        };

        /*       var problemDetails = factory.CreateProblemDetails(
                 httpContext,
                 type: url,
                 statusCode: statusCode,
                 title: title,
                 detail: message
             ); */

        problem.Extensions["errorType"] = error.GetType().Name;
        problem.Extensions["traceId"] = System.Diagnostics.Activity.Current?.Id ?? Guid.NewGuid().ToString();

        return new ObjectResult(problem) { StatusCode = statusCode, ContentTypes = { "application/problem+json" } };
    }
}

