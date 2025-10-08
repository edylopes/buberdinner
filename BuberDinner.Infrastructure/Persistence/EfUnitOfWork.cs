

using BuberDinner.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace BuberDinner.Infrastructure.Persistence;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public EfUnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default, bool detectChange = true)
    {
        if (detectChange)
        {
            _context.ChangeTracker.DetectChanges();
        }
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
}