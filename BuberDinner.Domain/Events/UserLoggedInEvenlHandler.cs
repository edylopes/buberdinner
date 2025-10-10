

using System.Security.Cryptography;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Events.Interfaces;

namespace BuberDinner.Domain.Events
{
    public class UserLoggedInHandler : IDomainEventHandler<UserLoggedInEvent>
    {
        private readonly IRepository<RefreshToken> _tokenRepository;
        public UserLoggedInHandler(IRepository<RefreshToken> tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }
        public async Task Handle(UserLoggedInEvent domainEvent)
        {
            byte[] randomBytes = GenerateRandomBytes();
            var token = new RefreshToken(token: Convert.ToBase64String(randomBytes), expires: DateTime.UtcNow.AddDays(7), domainEvent.UserId);
            await _tokenRepository.AddAsync(token);
        }

        private static byte[] GenerateRandomBytes(byte length = 64)
        {
            if (length > 255)
                throw new ArgumentOutOfRangeException(
                    nameof(length),
                    "Length must be less than or equal to 255."
                );

            var randomBytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return randomBytes;
        }



    }
}