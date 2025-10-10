

using BuberDinner.Application.Common.Interfaces.Persistence.Users;
using BuberDinner.Domain.Entities;


namespace BuberDinner.Application.Common.Interfaces.Persistence
{
    public interface IDinnerRepository : IRepository<Dinner>
    {
        Task<List<Dinner>> ListUserDinnersAsync(Guid id);
    }
}
