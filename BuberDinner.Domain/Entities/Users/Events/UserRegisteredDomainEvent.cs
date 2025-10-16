
using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.users.Events;

public record UserRegisteredDomainEvent(Guid userId, string email) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}