

using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.Users.Events;

public class UserLoggedInEvent : IDomainEvent
{
    public Guid UserId { get; }

    public DateTime OccurredOn => throw new NotImplementedException();

    public UserLoggedInEvent(Guid userId) => UserId = userId;
}