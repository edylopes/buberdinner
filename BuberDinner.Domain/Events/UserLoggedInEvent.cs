

using BuberDinner.Domain.Events.Interfaces;

namespace BuberDinner.Domain.Events;

public class UserLoggedInEvent : IDomainEvent
{
    public Guid UserId { get; }
    public UserLoggedInEvent(Guid userId) => UserId = userId;
}