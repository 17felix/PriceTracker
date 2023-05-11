using CleanArchitecture.Application.Common.Interfaces.Extensions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;
public class ForwardingDbContext : AuditableDbContextBase
{
    private readonly string connectionString;
    //private readonly IConnectionStringProvider connectionStringProvider;

    // TABLES
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Product> Products { get; }
    DbSet<Supplier> Suppliers { get; }
    DbSet<Tenant> Tenants { get; }

    // VIEWS
    /*public DbQuery<FoundCompanyViewEntity> FoundCompanies { get; set; }
    public DbQuery<BudgetTotalsReportEntity> BudgetTotalsReport { get; set; }*/

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.SetDecimalPrecision(8);
        modelBuilder.RemoveCascadeDeleteBehavior();

        Type[] excludedTypes =
        {
                typeof(Tenant),
                /*typeof(FoundOperationalFileTransportStopViewEntity),
                typeof(FoundBudgetDetailsPerFileEntity),
                typeof(FoundOperationalFileViewEntity),
                typeof(FoundOutgoingInvoicesViewEntity),
                typeof(FoundBudgetPerActivityCodeEntity),
                typeof(FoundOperationalSummaryPerUserEntity),
                typeof(BudgetTotalsReportEntity)*/
            };
        modelBuilder.RegisterSoftDeleteQueryFilter(excludedTypes);
        //modelBuilder.HasDefaultSchema("dbo");

        //modelBuilder.HasDbFunction(this.GetType().GetMethod(nameof(DamerauLevenshteinComparison), new[] { typeof(string), typeof(string), typeof(int?) })).HasName("DamerauLevenshteinComparison");

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseLazyLoadingProxies();

        /*if (connectionStringProvider != null)
        {
            string tenantConnectionString = connectionStringProvider.GetAsync<ForwardingDbContext>(CancellationToken.None).GetAwaiter().GetResult();
            optionsBuilder.UseSqlServer(tenantConnectionString);
        }*/
        //optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);

        base.OnConfiguring(optionsBuilder);
    }

    public void RejectChanges()
    {
        foreach (var entry in ChangeTracker.Entries().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
    }
}
