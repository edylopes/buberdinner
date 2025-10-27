using BuberDinner.Application.Common.Dto.Dinners;
using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Domain.Entities.Dinners;

namespace BuberDinner.Application.Common.Interfaces.Persistence.Dinners;

public interface IDinnerRepository : IRepository<Dinner>
{
    Task<List<DinnerDto>> ListUserDinnersAsync(Guid userId, bool active = true);
    Task<DinnerDto?> GetByIdAsync(Guid id);

}

