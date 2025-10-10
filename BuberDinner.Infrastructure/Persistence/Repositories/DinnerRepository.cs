
using BuberDinner.Domain.Entities;
using BuberDinner.Infrastructure.Persistence.Repositories.Context;

namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class DinnerRepository : RepositoryBase<Dinner>, IDinnerRepository
{
    public DinnerRepository(AppDbContext context) : base(context) { }
    public async Task<List<Dinner>> ListUserDinnersAsync(Guid userId)
    {
        return _context.Dinners.Where(d => d.HostId == userId).ToList();
    }
    public async Task<Dinner?> GetByIdAsync(Guid id)
    {
        return await _context.Dinners.FindAsync(id).AsTask();
    }
}

