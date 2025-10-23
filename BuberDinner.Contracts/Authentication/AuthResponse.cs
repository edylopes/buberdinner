
namespace BuberDinner.Contracts.Authentication;

public record AuthResponse(Guid Id, string Email, string FirstName, string LastName, string[] Roles);
