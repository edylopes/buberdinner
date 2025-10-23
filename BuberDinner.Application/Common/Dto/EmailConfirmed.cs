namespace BuberDinner.Application.Common.Dto;

public record EmailConfirmed(string Name, Guid UserId, string Message = "Email confirmed successfully");