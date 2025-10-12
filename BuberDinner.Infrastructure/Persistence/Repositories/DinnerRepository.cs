
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Persistence.Dinners;
using BuberDinner.Domain.Entities;
using BuberDinner.Infrastructure.Persistence.Repositories.Context;


namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class DinnerRepository : RepositoryBase<Dinner>, IDinnerRepository
{
    public DinnerRepository(AppDbContext context) : base(context) { }

    public async Task<Dinner?> GetByIdAsync(Guid id)
    {
        return await _context.Dinners
            .Include(u => u.Host)
            .FirstOrDefaultAsync(u => u.Id == id);
    }


    public List<Dinner> ListDinnersAsync()
    {
        return _context.Dinners
          .Include(d => d.Host)
          .ToList();
    }
    public async Task<List<Dinner>> ListUserDinnersAsync(Guid id, bool active)
    {
        var dinners = _context.Dinners
                       .AsNoTracking()
                       .Include(d => d.Guests)
                       .Include(d => d.Host)
                       .Where(d => active ? d.HostId == id && d.IsActive : d.HostId == id)
                       .ToList();

        return dinners;
    }
    public async Task<Dinner> GetDinnersActiveAsync()
    {

        throw new NotImplementedException();
    }

}

