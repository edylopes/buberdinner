

using BuberDinner.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BuberDinner.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{


    abstract ChangeTracker ChangeTracker { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default, bool detectChange = true);
    Task RollbackAsync(CancellationToken cancellationToken = default);
    IReadOnlyCollection<IDomainEvent> CollectDomainEvents();

}