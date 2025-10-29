using System.Transactions;

using BuberDinner.Domain.Common.Interfaces;
using BuberDinner.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace BuberDinner.Infrastructure.Persistence;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly List<IDomainEvent> _collectedEvents = new();
    private IDbContextTransaction? _transaction;
    public ChangeTracker ChangeTracker => _context.ChangeTracker;
    public bool HasActiveTransaction => _transaction != null;
    public EfUnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    public async Task CommitAsync(CancellationToken ct = default, bool detectChange = false)
    {

        if (detectChange)
            _context.ChangeTracker.DetectChanges();
        try
        {
            await _context.SaveChangesAsync(ct);
            if (_transaction is not null)
                await _transaction.CommitAsync(ct);
        }
        catch
        {
            if (_transaction != null)
                await _transaction.RollbackAsync(ct);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {

        if (HasActiveTransaction)
            await _transaction?.RollbackAsync()!;
    }


    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(ct);
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