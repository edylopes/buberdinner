

namespace BuberDinner.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default, bool detectChange = true);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}