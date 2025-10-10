using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities.Users;

namespace BuberDinner.Application.Common.Interfaces.Persistence;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task AddRefreshTokenAsync(Guid userId, RefreshToken refreshToken);
}
