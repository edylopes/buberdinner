using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities.Users;

namespace BuberDinner.Application.Authentication.Common;

public record AuthenticationResult(User User, string AccessToken, string RefreshToken);
