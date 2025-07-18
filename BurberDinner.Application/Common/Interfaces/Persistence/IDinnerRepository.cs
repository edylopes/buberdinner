

using BurberDinner.Domain.Entities;

namespace BurberDinner.Application.Common.Interfaces.Persistence
{
    public interface IDinnerRepository : IRepository<Dinner, Guid>
    {
        Task<List<Dinner>> ListUserDinnersAsync(Guid id);
    }
}