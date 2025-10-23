using BuberDinner.Application.Common.Dto.Dinners;
using BuberDinner.Application.Common.Interfaces.Persistence.Dinners;
using BuberDinner.Domain.Entities;
using BuberDinner.Infrastructure.Persistence.Context;
using MapsterMapper;


namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class DinnerRepository : RepositoryBase<Dinner>, IDinnerRepository
{
    public DinnerRepository(AppDbContext context, IMapper mapper) :
          base(context, mapper) { }

    public async Task<DinnerDto?> GetByIdAsync(Guid id)
    {
        var dinner =  await Context.Dinners
            .Include(u => u.Host)
            .Where(d => d.Id == id)
            .Select(d => Mapper.Map<DinnerDto>(d))
            .FirstOrDefaultAsync();

        return dinner;
    }
    
    public async Task<List<DinnerDto>> ListUserDinnersAsync(Guid userId, bool active = true)
    {
        var dinnersDto = await  Context.Dinners
                       .AsNoTracking()
                       .Include(d => d.Guests)
                       .Include(d => d.Host)
                       .Where(d => active ? d.HostId == userId && d.IsActive : d.HostId == userId)
                       .Select(dinners =>  Mapper.Map<DinnerDto>(dinners))
                       .ToListAsync(); //add-hoc query

        return  dinnersDto;
    }
}


