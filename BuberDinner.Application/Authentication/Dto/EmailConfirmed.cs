namespace BuberDinner.Application.Authentication.Dto;

public record EmailConfirmed(string name, Guid userId, string message = "Email confirmed successfully");