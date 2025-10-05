

namespace BuberDinner.Domain.Common.Errors;

public abstract record AppError(int statusCode, string url, string message, string? title = null);

public record DuplicatedEmailError(string url = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8")
    : AppError(409, url, "User with email already exist");

public record UserRoleNotAllowedError(string url = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1")
    : AppError(400, url, "User Role not allowed");

public record UserNotFoundError(string url = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4")
    : AppError(404, url, "User not found");


