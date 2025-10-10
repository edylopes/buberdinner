using System.Linq.Expressions;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities.Users;
using BuberDinner.Infrastructure.Persistence.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{

    public UserRepository(AppDbContext context)
    : base(context)
    {

    }
    public async Task<User?> GetByIdAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    public Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    public Task Add<T>(T entity)
    {
        var entry = _context.Entry(entity);
        entry.State = EntityState.Added;
        return Task.CompletedTask;
    }
    public async Task AddRefreshTokenAsync(User user)

    {
        /* EF Core percorrer todas as entidades rastreadas e atualizar seus estados (Modified, Added, Deleted, etc.) com base nas alterações nas propriedades. */
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");
        }

    }

}

