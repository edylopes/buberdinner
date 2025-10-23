using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities.Users;

namespace BuberDinner.Application.Common.Interfaces.Persistence.Users;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task<User?> GetByIdAsync(Guid id);
    void UpdateEmailConfirmed(User user);
}
