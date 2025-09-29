
namespace BuberDinner.Domain.Common.Errors;

public abstract record AppError(string message, string? title = null);

public record DuplicatedEmailError()
    : AppError("User with email already exist");

public record UserRoleNotAllowedError()
    : AppError("User Role not allowed");

public record UserNotFoundError()
    : AppError("User not found", "User not found");
