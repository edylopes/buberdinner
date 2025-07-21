namespace BurberDinner.Contracts.Authentication;

public record AuthenticationResponse(Guid id,
    string firstName,
    string lastName,
    string email,
    string zole);