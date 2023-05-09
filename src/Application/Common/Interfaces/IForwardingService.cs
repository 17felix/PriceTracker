using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;
public interface IForwardingService<TEntity> : IService<TEntity>
    where TEntity : class, ISoftDeletable, IIdentifiable<long>
{
    /// <summary>
    ///     Deletes the provided entity, optionally permanently.
    /// </summary>
    /// <param name="entityToRemove">The entity to delete.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <param name="hard"><c>True</c> to hard delete (permanent) the entity, otherwise <c>false</c>.</param>
    /// <returns></returns>
    Task DeleteAsync(TEntity entityToRemove, bool hard = false, CancellationToken cancellationToken = default(CancellationToken));

    Task DeleteAsync(IReadOnlyCollection<long> entitiesToDelete);

    Task DeleteAsync(TEntity[] entitiesToRemove, bool hard, CancellationToken cancellationToken = default(CancellationToken));

    Task DeleteAsync(long id);

    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default(CancellationToken));
    Task LoadForeignPropertiesAsync(TEntity entity, CancellationToken cancellationToken = default);

}

public abstract class ForwardingServiceBase<TEntity> : ServiceBase<ForwardingDbContext, TEntity>, IForwardingService<TEntity>
    where TEntity : class, ISoftDeletable, IIdentifiable<long>
{
    protected ForwardingServiceBase(ForwardingDbContext dbContext)
        : base(dbContext)
    { }

    public virtual Task LoadForeignPropertiesAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(IReadOnlyCollection<long> entityKeysToDelete)
    {
        TEntity[] entitiesToDelete = entityKeysToDelete.Select(key => DbContext.Set<TEntity>().Find(key)).Where(e => e != null).ToArray();
        foreach (TEntity entity in entitiesToDelete)
        {
            await DeleteAsync(entity, false);
        }
    }

    public async Task DeleteAsync(TEntity entityToRemove, bool hard, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (hard)
        {
            DbContext.Set<TEntity>().Remove(entityToRemove);
        }
        else
        {
            if (DbContext.Entry(entityToRemove).State == EntityState.Detached)
            {
                DbContext.Set<TEntity>().Attach(entityToRemove);
            }

            entityToRemove.IsDeleted = true;
        }

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id)
    {
        TEntity entity = DbContext.Set<TEntity>().Find(id);
        if (entity != null)
        {
            await DeleteAsync(entity, false);
        }
    }

    public Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
    {
        return DbContext.Set<TEntity>().AnyAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task DeleteAsync(TEntity[] entitiesToRemove, bool hard, CancellationToken cancellationToken = default(CancellationToken))
    {
        foreach (TEntity entityToRemove in entitiesToRemove ?? new TEntity[0])
        {
            await DeleteAsync(entityToRemove, hard, cancellationToken);
        }
    }
}
