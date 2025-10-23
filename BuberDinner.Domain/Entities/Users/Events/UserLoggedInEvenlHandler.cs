using System.Security.Cryptography;
using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.Entities.Users.Events
{
    public class UserLoggedInHandler : IDomainEventHandler<UserLoggedInEvent>
    {

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

        public Task Handle(UserLoggedInEvent domainEvent)
        {

            throw new NotImplementedException();
        }
    }
}