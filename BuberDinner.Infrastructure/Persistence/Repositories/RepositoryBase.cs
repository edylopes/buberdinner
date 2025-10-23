using System.Linq.Expressions;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Infrastructure.Persistence.Context;
using MapsterMapper;

namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext Context;
    protected readonly IMapper Mapper;

    protected readonly DbSet<T> DbSet;

    public RepositoryBase(AppDbContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
        DbSet = context.Set<T>();
    }

    // Add (EF marca como Added automaticamente)
    public virtual async Task AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        await DbSet.AddAsync(entity);
    }

    // Força o estado Added (útil se a entidade já tem Id preenchido)
    public void MarkAsAdded<TE>(TE entity)
    {
        var entry = Context.Entry(entity!);
        entry.State = EntityState.Added;
    }
    public void Update(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        var entry = Context.Entry(entity);
        if (entry.State == EntityState.Detached)
            DbSet.Attach(entity);

        entry.State = EntityState.Modified;
    }
    public void Delete(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException($"{entity?.GetType().Name} is required", nameof(entity));
        var entry = Context.Entry(entity);

        if (entry.State == EntityState.Detached)
            DbSet.Attach(entity);

        DbSet.Remove(entity);

    }
    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));
        return await DbSet.AnyAsync(predicate);
    }
    
}