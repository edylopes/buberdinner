using BuberDinner.Domain.Entities.Users;
using BuberDinner.Domain.Exceptions;
using BuberDinner.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BuberDinner.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException("User is required", nameof(user));

        if (_context.Users.Any(u => u.Email == user.Email))
            throw new InvalidOperationException("User with this email already exists");

        await _context.Users.AddAsync(user);

    }

    public async Task<User?> GetByIdAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task DeleteAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        _context.Users.Remove(user);

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id)
            ?? throw new InvalidOperationException("User not found");

        _context.Users.Remove(existingUser);
    }

    public async Task UpdateAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        await _context.SaveChangesAsync();
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

    public async Task AddRefreshTokenAsync(Guid userId, RefreshToken refreshToken)
    {
        var user = await _context.Users
           .Include(u => u.RefreshTokens)
           .FirstAsync(u => u.Id == userId);

        user.AddRefreshToken(refreshToken);
        _context.Entry(refreshToken).State = EntityState.Added;
    }
}
