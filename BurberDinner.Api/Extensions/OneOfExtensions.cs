
using BurberDinner.Api.Helpers;
using BurberDinner.Api.Utils;
using BurberDinner.Application.Errors;
using BurberDinner.Application.Services.Authentication;
using BurberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace BurberDinner.Api.Extensions;


public static class OneOfExtensions
{

    private const string BadRequestUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";

    public static IActionResult ToActionResult<TSuccess>(this OneOf<TSuccess, AppError> result, ControllerBase controller)
    {

        return result.Match<IActionResult>(
        user =>
            {
                var response = user as AuthenticationResult;

                if (response is null)
                    return controller.BadRequest("Invalid Result Type");
                controller.Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
                controller.Response.Headers["Authorization"] = $"Bearer {response.Token}";

                return controller.Ok(new AuthenticationResponse(
                     response.Id, response.FirstName, response.LastName, response.Email, response.Role));
            },
         error =>
         {
             var problemDetails = new ProblemDetails
             {
                 Type = error.typeUrl,
                 Title = "Validation Errors",
                 Detail = error.message,
                 Status = error.statusCode,

             };

             return new OkObjectResult(problemDetails);

         });
    }
}

/* //Register defaults error mappings  //TODO
// ErrorMappingRegistry.RegisterDefaults();
var error = ErrorMappingRegistry.GetMapping(err);

var (statusCode, errorMessage, typeUrl) = error
      ?? (StatusCodes.Status400BadRequest, "Unknown error", BadRequestUrl);  */