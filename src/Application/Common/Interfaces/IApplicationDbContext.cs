using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext 
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Product> Products { get; }
    /*DbSet<DirtyProduct> DirtyProducts { get; }
    DbSet<Supplier> Suppliers { get; }
    DbSet<Tenant> Tenants { get; }*/

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
