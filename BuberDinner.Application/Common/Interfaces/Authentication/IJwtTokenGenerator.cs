using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities.Users;

namespace BuberDinner.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    public (string Token, RefreshToken refreshToken) GenerateTokens(User user);

}
