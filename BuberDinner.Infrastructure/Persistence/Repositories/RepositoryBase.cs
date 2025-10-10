

using System.Linq.Expressions;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Infrastructure.Persistence.Repositories.Context;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;
    public RepositoryBase(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    // Add (EF marca como Added automaticamente)
    public virtual async Task AddAsync(T entity)

    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        await _dbSet.AddAsync(entity);
    }
    // Força o estado Added (útil se a entidade já tem Id preenchido)
    public virtual Task Added<TE>(TE entity)
    {
        var entry = _context.Entry(entity);
        entry.State = EntityState.Added;
        return Task.CompletedTask;
    }
    public virtual Task UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        var entry = _context.Entry(entity);
        if (entry.State == EntityState.Detached)
            _dbSet.Attach(entity);

        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
    public virtual Task DeleteAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException($"{entity?.GetType().Name} is required", nameof(entity));
        var entry = _context.Entry(entity);

        if (entry.State == EntityState.Detached)
            _dbSet.Attach(entity);

        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }
    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));
        return await _dbSet.AnyAsync(predicate);
    }
}
