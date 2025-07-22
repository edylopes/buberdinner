using BuberDinner.Domain.Entities;

namespace BuberDinner.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new();

    public Task AddAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException("User is required", nameof(user));

        if (_users.Any(u => u.Email == user.Email))
            throw new InvalidOperationException("User with this email already exists");

        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task<User?> GetByIdAsync(string email)
    {
        return Task.FromResult(_users.Find(u => u.Email == email));
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByEmailAsync(string email)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Email == email)!);
    }

    public Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
