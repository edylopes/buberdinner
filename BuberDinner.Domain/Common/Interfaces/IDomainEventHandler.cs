

using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.Events.Interfaces;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent domainEvent);
}
public interface IDomainEventDispatcher
{
    Task Dispatch(IDomainEvent domainEvent);
}
