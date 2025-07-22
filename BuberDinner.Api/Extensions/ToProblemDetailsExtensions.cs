using BuberDinner.Application.Errors;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Extensions;

public static class ToProblemDetailsExtensions
{
    public static IActionResult ToProblemDetails(this AppError error)
    {
        var problemDetails = new ProblemDetails
        {
            Type = error.TypeUrl,
            Title = "Validations errors",
            Detail = error.Message,
            Status = error.StatusCode,
        };

        return new ObjectResult(problemDetails) { StatusCode = error.StatusCode };
    }
}
