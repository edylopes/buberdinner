using BuberDinner.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuberDinner.Infrastructure.Persistence
{
    public class DinnerRepository : IDinnerRepository
    {
        private readonly BurberDinnerDbContext _context;

        public DinnerRepository(BurberDinnerDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Dinner dinner)
        {
            _context.Dinners.Add(dinner);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Dinner>> ListUserDinnersAsync(Guid userId)
        {
            return await _context.Dinners.Where(d => d.HostId == userId).ToListAsync();
        }

        public Task<Dinner?> GetByIdAsync(Guid id)
        {
            return _context.Dinners.FindAsync(id).AsTask();
        }

        public Task UpdateAsync(Dinner dinner)
        {
            _context.Dinners.Update(dinner);
            return _context.SaveChangesAsync();
        }
    }
}
