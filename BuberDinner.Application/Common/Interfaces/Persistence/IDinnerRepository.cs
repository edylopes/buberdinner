

using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities.Users;

namespace BuberDinner.Application.Common.Interfaces.Persistence
{
    public interface IDinnerRepository : IRepository<Dinner>
    {
        Task<List<Dinner>> ListUserDinnersAsync(Guid id);
    }
}
