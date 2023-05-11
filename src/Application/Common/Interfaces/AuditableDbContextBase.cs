using CleanArchitecture.Application.Common.Interfaces.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;
public abstract class AuditableDbContextBase : DbContext
{
    public bool IsAuditingEnabled { get; set; } = true;


    protected AuditableDbContextBase()
    {
    }

    protected AuditableDbContextBase(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    /*
    private void TryStampAuditProperties()
    {
        if (IsAuditingEnabled)
        {
            this.StampAuditProperties();
        }
    }
    */

    public override int SaveChanges()
    {
        //TryStampAuditProperties();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        //TryStampAuditProperties();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        //TryStampAuditProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        //TryStampAuditProperties();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
