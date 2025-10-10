using System.Linq.Expressions;

namespace BuberDinner.Application.Common.Interfaces.Persistence.Users;

public interface IRepository<T> where T : class

{
    Task Added<TE>(TE entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task AddAsync(T entity);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}

