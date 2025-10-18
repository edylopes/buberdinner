
using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.users.Events;

public record UserRegisteredDomainEvent(Guid userId, string email, string name) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}