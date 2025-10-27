

using BuberDinner.Api.Common.Errors;
using BuberDinner.Domain.Common.Errors;
using OneOf;

namespace BuberDinner.Api.Extensions;

public static class OneOfExtensions
{
  public static IActionResult ToActionResponse<T>(this OneOf<T, AppError> result)
  {
    return result.Match(
      sucess => new OkObjectResult(sucess),
      error => ErrorResults.FromError(error)
    );
  }
}
