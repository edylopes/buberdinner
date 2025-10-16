

using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.Events.Interfaces;

public interface IHasDomainEvents
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvents();

}
