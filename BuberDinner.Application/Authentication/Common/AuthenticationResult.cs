using BuberDinner.Domain.Entities.Users;

namespace BuberDinner.Application.Authentication.Common;

public record AuthenticationResult(User user, string accessToken, string refreshToken);
