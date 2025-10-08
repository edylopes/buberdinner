
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities.Users;
public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task AddRefreshTokenAsync(Guid userId, RefreshToken refreshToken);
}
