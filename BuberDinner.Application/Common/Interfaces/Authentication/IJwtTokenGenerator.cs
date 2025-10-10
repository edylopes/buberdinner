using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    public (string Token, RefreshToken refreshToken) GenerateTokens(User user);

}
