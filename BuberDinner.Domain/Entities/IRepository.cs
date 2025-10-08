namespace BuberDinner.Domain.Entities;

public interface IRepository<Entity, TId>
    where Entity : class
{
    public Task AddAsync(Entity user);
    public Task<Entity> GetByIdAsync(TId id);
}
