namespace BuberDinner.Contracts.Authentication;

public record AuthResponse(
    Guid id,
    string firstName,
    string lastName,
    string email,
    string role
);
