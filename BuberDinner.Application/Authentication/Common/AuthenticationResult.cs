using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Authentication.Commands.Common;

public record AuthenticationResult(User User, string AccessToken, string RefreshToken);
