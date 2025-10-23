

using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.Entities.Users.Events;

public class UserLoggedInEvent : IDomainEvent
{
    public Guid UserId { get; }

    public DateTime OccurredOn => throw new NotImplementedException();

    public UserLoggedInEvent(Guid userId) => UserId = userId;
}