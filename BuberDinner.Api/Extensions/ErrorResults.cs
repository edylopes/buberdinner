using BuberDinner.Api.Common.Errors;
using BuberDinner.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BuberDinner.Api.Extensions.Auth
{
    public static class ErrorResults
    {
        public static IActionResult FromError(AppError error, HttpContext httpContext)
        {
            var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var (statusCode, url, message) = ErrorMapper.ToError(error);

            var problem = factory.CreateProblemDetails(
                httpContext,
                statusCode: statusCode,
                detail: message,
                type: url,
                instance: httpContext.Request.Path
            );

            return new ObjectResult(problem) { StatusCode = statusCode };
        }
    }

}