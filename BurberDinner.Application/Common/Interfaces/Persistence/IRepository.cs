namespace BurberDinner.Application.Common.Interfaces.Persistence;

public interface IRepository<Entity, TId> where Entity : class
{
    public void Add(Entity user);
    public Entity? GetUserByEmail(string email);
}