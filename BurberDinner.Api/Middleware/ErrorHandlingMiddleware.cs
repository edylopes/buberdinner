

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace BurberDinner.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var result = JsonSerializer.Serialize(new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Status = context.Response.StatusCode,
                Title = "An error occurred while processing your request.",
                Detail = exception.Message,
                Instance = context.Request.Path

            });
            return context.Response.WriteAsync(result);
        }
    }
}