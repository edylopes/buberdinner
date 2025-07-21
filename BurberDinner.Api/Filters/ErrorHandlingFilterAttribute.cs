
using BurberDinner.Api.Utils;
using BurberDinner.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BurberDinner.Api.Filters
{
    /// <summary>
    /// ErrorHandlingFilterAttribute is an ASP.NET Core filter  that handles exceptions thrown by controllers.
    /// It captures exceptions, retrieves the appropriate HTTP status code and error details from the ExceptionMappingRegistry,
    /// </summary>
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {

        private readonly ILogger<ErrorHandlingFilterAttribute> _logger;

        /// <summary>
        /// Error URL is used to provide a link to the documentation for server errors RCF.
        /// </summary>
        const string SERVER_ERROR_URL = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";

        public ErrorHandlingFilterAttribute(ILogger<ErrorHandlingFilterAttribute> logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {

            /*  var exception = ExceptionMappingRegistry.GetMapping(context.Exception); */
            context.HttpContext.Response.ContentType = "application/json";

            context.Result = context.Exception switch
            {
                UserRoleNotAllowedException ex => new ObjectResult(new ProblemDetails
                {
                    Type = "https://httpstatuses.com/409",
                    Title = "User Role Not Allowed",
                    Status = StatusCodes.Status403Forbidden,
                    Detail = $"User Role not Allowed.",
                }),
                _ => new ObjectResult(new ProblemDetails
                {
                    Type = SERVER_ERROR_URL,
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "An unexpected error occurred while processing your request.",
                    Instance = context.HttpContext.Request.Path.Value
                })
            };
            context.HttpContext.Response.StatusCode = context.Result is ObjectResult objectResult ? objectResult.StatusCode ?? StatusCodes.Status500InternalServerError : StatusCodes.Status500InternalServerError;
            context.ExceptionHandled = true;


        }
    }
}