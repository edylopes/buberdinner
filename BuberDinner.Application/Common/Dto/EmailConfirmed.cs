namespace BuberDinner.Application.Common.Dto;

public record EmailConfirmed(string Message = "Email confirmed successfully");
public record EmailNotConfirmetionError(string Name, Guid UserId, string Message = "User Not found");