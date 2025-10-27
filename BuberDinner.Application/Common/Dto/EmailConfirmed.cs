namespace BuberDinner.Application.Common.Dto;

public record EmailConfirmed(string email, string message = "Email confirmed successfully");