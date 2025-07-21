using BurberDinner.Domain.Entities;

namespace BurberDinner.Application.Services.Authentication;

public record AuthenticationResult(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string Token,
        string RefreshToken,
        string Role
    );