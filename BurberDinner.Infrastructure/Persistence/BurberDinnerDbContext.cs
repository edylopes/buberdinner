
using BurberDinner.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BurberDinner.Infrastructure.Persistence;

public class BurberDinnerDbContext : DbContext
{

    public DbSet<Host> Hosts { get; set; }
    public DbSet<Dinner> Dinners { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BurberDinnerDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

}

