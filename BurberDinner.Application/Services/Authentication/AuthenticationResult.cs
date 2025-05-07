using BurberDinner.Domain.Entities;

namespace BurberDinner.Application.Services.Authentication;

public record AuthenticationResult(
    User user,
    string? Token
);