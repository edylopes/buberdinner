

using System.Linq.Expressions;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities.Users;
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task AddRefreshTokenAsync(User user);
    Task Add<T>(T entity);
    Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);

}
