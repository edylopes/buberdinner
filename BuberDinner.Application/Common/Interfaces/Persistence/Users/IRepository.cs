using System.Linq.Expressions;

namespace BuberDinner.Application.Common.Interfaces.Persistence.Users;

public interface IRepository<T> where T : class

{
    void MarkAsAdded<TE>(TE entity);
    void Update(T entity);
    void Delete(T entity);
    Task AddAsync(T entity);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}

