using BuberDinner.Domain.Entities;

namespace BuberDinner.Infrastructure.Persistence.Repositories
{
    public class InMemoryDinnerDatabase : IDinnerRepository
    {
        private readonly List<Dinner> _dinners = new();

        public Task AddAsync(Dinner dinner)
        {
            _dinners.Add(dinner);
            return Task.CompletedTask;
        }

        public Task<List<Dinner>> ListUserDinnersAsync(Guid userId)
        {
            return Task.FromResult(_dinners.Where(dinner => dinner.Host.UserId == userId).ToList());
        }

        public Task<Dinner?> GetByIdAsync(Guid id)
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

        public Task<Dinner> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
