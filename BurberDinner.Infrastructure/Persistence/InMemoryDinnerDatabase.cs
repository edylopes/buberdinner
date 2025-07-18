
using BurberDinner.Domain.Entities;

namespace BurberDinner.Infrastructure.Persistence
{
    public class InMemoryDinnerDatabase : IDinnerRepository
    {
        private readonly List<Dinner> _dinners = new();

        public Task AddAsync(Dinner dinner)
        {
            _dinners.Add(dinner);
            return Task.CompletedTask;
        }

        public async IAsyncEnumerable<Dinner> ListUserDinnersAsync(Guid userId)
        {
            foreach (var dinners in _dinners.Where(dinner => dinner.Host.UserId == userId))
            {
                yield return dinners;
                await Task.Yield(); // Ensure asynchronous operation
            }
        }
        public Task<Dinner> GetByIdAsync(Guid id)
        {
            var dinner = _dinners.FirstOrDefault(d => d.Id == id);
            return Task.FromResult(dinner);
        }

        public Task UpdateAsync(Dinner dinner)
        {
            var existingDinner = _dinners.FirstOrDefault(d => d.Id == dinner.Id);
            if (existingDinner != null)
            {
                existingDinner = dinner; // Update logic here
            }
            return Task.CompletedTask;
        }

        public Dinner GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}