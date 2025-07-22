namespace BuberDinner.Contracts.Authentication;

public record AuthenticationResponse(
    Guid id,
    string firstName,
    string lastName,
    string email,
    string role
);
