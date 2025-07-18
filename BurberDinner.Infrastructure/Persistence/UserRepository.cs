using BurberDinner.Domain.Entities;

namespace BurberDinner.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new();
    public Task Add(User user, string token)
    {
        user.Token = token;
        _users.Add(user);
        return Task.CompletedTask;
    }

    public async Task AddAsync(User user)
    {
        _users.Add(user);
        await Task.CompletedTask;
    }

    public User GetUserByEmail(string email) => _users.SingleOrDefault(x => x.Email == email);
    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    public Task<User> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }
    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
}