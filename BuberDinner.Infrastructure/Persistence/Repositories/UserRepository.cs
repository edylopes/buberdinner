
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities.Users;
using BuberDinner.Infrastructure.Persistence.Context;
using MapsterMapper;


namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(AppDbContext context, IMapper mapper) : base(context, mapper) { }
    public async Task<User?> GetByIdAsync(Guid id)
    => await Context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByEmailAsync(string email)
    => await Context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email);
    public Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        throw new NotImplementedException();
    }

    public void UpdateEmailConfirmed(User user)
    {
        Context.Attach(user);
        Context.Entry(user).Property(u => u.EmailConfirmed).IsModified = true;
        Context.Entry(user).Property(u => u.UpdatedAt).IsModified = true;
    }

}

