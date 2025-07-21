

namespace BurberDinner.Domain.DomainErrors;

/// <summary>
/// Contains errors definitions related to user operations.
/// </summary>
public abstract record AppError;    
public record DuplicatedEmailError : AppError;
public record UserRoleNotAllowedError(string role) : AppError;

