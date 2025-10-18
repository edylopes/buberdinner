
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Domain.Entities;
using BuberDinner.Infrastructure.Persistence.Repositories.Context;


namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }
    public async Task<User?> GetByIdAsync(Guid id)
    => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByEmailAsync(string email)
    => await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email);
    public Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        throw new NotImplementedException();
    }

    public void UpdateEmailConfirmed(User user)
    {
        _context.Attach(user);
        _context.Entry(user).Property(u => u.EmailConfirmed).IsModified = true;
        _context.Entry(user).Property(u => u.UpdatedAt).IsModified = true;
    }

}

