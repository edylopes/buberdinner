namespace BurberDinner.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password
);