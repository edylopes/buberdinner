namespace BuberDinner.Contracts.Authentication;

public record AuthResponse(Guid id, string email, string firstName, string lastName, string[] roles);
