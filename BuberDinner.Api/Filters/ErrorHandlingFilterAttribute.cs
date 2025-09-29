using System.Data.Common;
using BuberDinner.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BuberDinner.Api.Filters;

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
                _logger.LogError("Database operation failed: {ex}", ex);
                ConfigureProblemDetails(problemDetails, "Infrastructure error", ex.Message);
                problemDetails.Status = StatusCodes.Status503ServiceUnavailable;
                break;
            case DomainException ex:
                _logger.LogError("Business Rule Violated{ex}", ex);
                ConfigureProblemDetails(problemDetails, "Domain Error", ex.Message);
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;

            default:
                _logger.LogError(
                    context.Exception,
                    "An internal error occurred on the server {traceId}",
                    context.HttpContext.TraceIdentifier
                );
                ConfigureProblemDetails(
                    problemDetails,
                    "An Internal Server Error Occurred",
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

    private void ConfigureProblemDetails(ProblemDetails problemDetails, string title, string detail)
    {
        problemDetails.Title = title;
        problemDetails.Detail = _env.IsDevelopment()
            ? detail
            : "An unexpected error occurred. Please contact support.";
    }

    private static ProblemDetails CreateBaseProblemDetails(HttpContext context) =>
        new ProblemDetails
        {
            Type = ServerErrorUrl,
            Instance = context.Request.Path.Value,
            /* Adction info traceIdIdentifier Important for tracking occurrences and investigating problems. */
            Extensions = { ["traceId"] = context.TraceIdentifier },

        };
}
