namespace BurberDinner.Application.Common.Interfaces.Persistence;

public interface IRepository<Entity, TId> where Entity : class
{
    public Task AddAsync(Entity user);
    public Entity GetUserByEmail(string email);
}