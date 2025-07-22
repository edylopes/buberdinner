using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Common.Interfaces.Persistence
{
    public interface IDinnerRepository : IRepository<Dinner, Guid>
    {
        Task<List<Dinner>> ListUserDinnersAsync(Guid id);
    }
}
