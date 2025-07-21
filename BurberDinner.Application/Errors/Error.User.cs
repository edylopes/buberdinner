

namespace BurberDinner.Application.Errors;

/// <summary>
/// Contains errors definitions related to user operations.
/// </summary>
/// 
/// 
public static class ErrorDefaults
{
        public const string TypeUrl = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
        public const int Conflict = 409;
        public const int BadRequest = 400;
}

public abstract record AppError(int statusCode, string typeUrl, string message);


public record DuplicatedEmailError() :
        AppError(ErrorDefaults.Conflict, ErrorDefaults.TypeUrl, "User with email Already exist");
public record UserRoleNotAllowedError()
        : AppError(ErrorDefaults.BadRequest, ErrorDefaults.TypeUrl, "User Role not Allowed");
public record ValidationError(Dictionary<string, string[]>? errors)
      : AppError(ErrorDefaults.BadRequest, ErrorDefaults.TypeUrl, "Validation Errors");