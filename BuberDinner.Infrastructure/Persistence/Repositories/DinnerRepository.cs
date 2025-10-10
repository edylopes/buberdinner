using BuberDinner.Domain.Entities.Users;
using BuberDinner.Infrastructure.Persistence.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class DinnerRepository : RepositoryBase<Dinner>, IDinnerRepository
{
    public DinnerRepository(AppDbContext context) : base(context) { }
    public async Task<List<Dinner>> ListUserDinnersAsync(Guid userId)
    {
        return _context.Dinners.Where(d => d.HostId == userId).ToList();
    }
    public Task<Dinner?> GetByIdAsync(Guid id)
    {
        return _context.Dinners.FindAsync(id).AsTask();
    }
}

