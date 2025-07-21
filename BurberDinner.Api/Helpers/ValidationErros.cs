using BurberDinner.Application.Errors;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BurberDinner.Api.Helpers;

public static class ValidationErrorHelper
{
    public static ValidationError FromModelState(ModelStateDictionary modelState)
    {
        var result = modelState
            .Where(e => e.Value?.Errors?.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        return new ValidationError(result);
    }

}