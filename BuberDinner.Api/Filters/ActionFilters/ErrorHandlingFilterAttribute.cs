using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BuberDinner.Api.Filters
{
    /// <summary>
    /// ErrorHandlingFilterAttribute is an ASP.NET Core filter  that handles exceptions thrown by controllers.
    /// It captures exceptions, retrieves the appropriate HTTP status code and error details from the ExceptionMappingRegistry,
    /// </summary>
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        private const string SERVER_ERROR_URL =
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
        private const string CONTENT_TYPE = "application/problem+json";
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

            context.HttpContext.Response.ContentType = CONTENT_TYPE;
            context.ExceptionHandled = true;
        }

        private void ConfigureProblemDetails(ProblemDetails details, string title, string detail)
        {
            details.Title = title;
            details.Detail = _env.IsDevelopment()
                ? detail
                : "An unexpected error occurred. Please contact support.";
        }

        private ProblemDetails CreateBaseProblemDetails(HttpContext context) =>
            new ProblemDetails
            {
                Type = SERVER_ERROR_URL,
                Instance = context.Request.Path.Value,
                Extensions = { ["traceId"] = context.TraceIdentifier },
            };
    }
}
