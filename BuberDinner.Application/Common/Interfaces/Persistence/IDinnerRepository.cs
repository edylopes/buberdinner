using BurberDinner.Domain.Entities;

namespace BuberDinner.Application.Common.Interfaces.Persistence
{
    public interface IDinnerRepository : IRepository<Dinner, Guid>
    {
        List<Dinner> ListUserDinnersAsync(Guid id);
    }
}
