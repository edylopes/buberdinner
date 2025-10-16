

using BuberDinner.Domain.Common;
using BuberDinner.Domain.Common.Interfaces;
using BuberDinner.Domain.Events.Interfaces;
using BuberDinner.Infrastructure.Persistence.Repositories.Context;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly List<IDomainEvent> _collectedEvents = new();
    private IDbContextTransaction? _transaction;
    public ChangeTracker ChangeTracker => _context.ChangeTracker;
    public EfUnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    public async Task CommitAsync(CancellationToken cancellationToken = default, bool detectChange = false)
    {


        await _context.SaveChangesAsync(cancellationToken);
        await _transaction?.CommitAsync(cancellationToken)!;

    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {

        await _transaction?.RollbackAsync()!;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }


    public void Dispose() => _transaction?.Dispose();

    public IReadOnlyCollection<IDomainEvent> CollectDomainEvents()
    {

        var trackedEvents = _context.ChangeTracker
              .Entries<IHasDomainEvents>()
              .SelectMany(e => e.Entity.DomainEvents)
              .ToList();

        // Junta eventos do contexto e os coletados manualmente
        var allEvents = _collectedEvents
            .Concat(trackedEvents)
            .ToList();

        // Limpa eventos do contexto para não republicar depois
        foreach (var entity in _context.ChangeTracker.Entries<IHasDomainEvents>())
            entity.Entity.ClearDomainEvents();

        _collectedEvents.Clear();

        return allEvents;
    }
}