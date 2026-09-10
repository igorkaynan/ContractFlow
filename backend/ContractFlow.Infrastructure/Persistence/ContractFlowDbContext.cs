using ContractFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContractFlow.Infrastructure.Persistence;

public class ContractFlowDbContext : DbContext
{
    public ContractFlowDbContext(DbContextOptions<ContractFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>()
            .Property(c => c.Value)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Contract>()
            .Property(c => c.Status)
            .HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
}