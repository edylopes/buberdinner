
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Entities.Users;
using Host = BuberDinner.Domain.Entities.Hosts.Host;

namespace BuberDinner.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
    public DbSet<Host> Hosts { get; set; }
    public DbSet<Dinner> Dinners { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
