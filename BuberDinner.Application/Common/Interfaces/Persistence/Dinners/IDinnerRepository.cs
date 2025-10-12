

using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Common.Interfaces.Persistence.Dinners;

public interface IDinnerRepository : IRepository<Dinner>
{
    Task<List<Dinner>> ListUserDinnersAsync(Guid id, bool active);
    Task<Dinner?> GetByIdAsync(Guid id);
    Task<Dinner> GetDinnersActiveAsync();
}

