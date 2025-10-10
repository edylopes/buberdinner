namespace BuberDinner.Domain.Entities;

public interface IRepository<T> where T : class

{
    public Task Added(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task AddAsync(T entity);
}

