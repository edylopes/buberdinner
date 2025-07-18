using BurberDinner.Domain.Entities;

namespace BurberDinner.Application.Common.Interfaces.Persistence;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
}
