using BuberDinner.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Extensions;

public static class ToProblemDetailsExtensions
{
    public static IActionResult ToProblemDetails(this AppError error) =>
        new ObjectResult(
            new ProblemDetails
            {
                Type = error.TypeUrl,
                Title = "Validation errors",
                Detail = error.Message,
                Status = error.StatusCode,
            }
        )
        {
            StatusCode = error.StatusCode
        };
}
