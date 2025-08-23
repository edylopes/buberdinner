using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BuberDinner.Api.Filters.ActionFilters
{
    /// <summary>
    /// ErrorHandlingFilterAttribute is an ASP.NET Core filter  that handles exceptions thrown by controllers.
    /// It captures exceptions, retrieves the appropriate HTTP status code and error details from the ExceptionMappingRegistry,
    /// </summary>
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        private const string ServerErrorUrl =
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";

        private const string ContentType = "application/problem+json";
        private readonly ILogger<ErrorHandlingFilterAttribute> _logger;
        private readonly IHostEnvironment _env;

        public ErrorHandlingFilterAttribute(
            ILogger<ErrorHandlingFilterAttribute> logger,
            IHostEnvironment env
        )
        {
            _logger = logger;
            _env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            var problemDetails = CreateBaseProblemDetails(context.HttpContext);
            problemDetails.Status = StatusCodes.Status500InternalServerError;

            switch (context.Exception)
            {
                case DbException ex:
                    _logger.LogError("Internal Server Error {ex}", ex);
                    ConfigureProblemDetails(problemDetails, "Infrastructure error", ex.Message);
                    break;
                default:
                    _logger.LogError(
                        context.Exception,
                        "An internal error occurred on the server {ex}",
                        context.HttpContext.TraceIdentifier
                    );
                    ConfigureProblemDetails(
                        problemDetails,
                        "Internal Server Error",
                        context.Exception.Message
                    );
                    break;
            }

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status,
            };

            problemDetails.Extensions["exceptionType"] = context.Exception.GetType().Name;
            problemDetails.Extensions["timestamp"] = DateTimeOffset.UtcNow;

            context.HttpContext.Response.ContentType = ContentType;
            context.ExceptionHandled = true;
        }

        private void ConfigureProblemDetails(ProblemDetails details, string title, string detail)
        {
            details.Title = title;
            details.Detail = _env.IsDevelopment()
                ? detail
                : "An unexpected error occurred. Please contact support.";
        }

        private static ProblemDetails CreateBaseProblemDetails(HttpContext context) =>
            new ProblemDetails
            {
                Type = ServerErrorUrl,
                Instance = context.Request.Path.Value,
                Extensions = { ["traceId"] = context.TraceIdentifier },
            };
    }
}
