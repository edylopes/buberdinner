
using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.Entities.Users.Events;

public record UserRegisteredDomainEvent(Guid UserId, string Email, string Name) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}