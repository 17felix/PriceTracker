﻿using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;
public interface IService<TEntity> where TEntity : IIdentifiable<int>
{
    /// <summary>
    ///     Retrieve an entity based on its identifier.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <typeparam name="TEntity">The type of entity to retrieve.</typeparam>
    Task<TEntity> GetAsync(int id);

    /// <summary>
    ///     Retrieves all entities matching the list of provided identifiers.
    /// </summary>
    /// <param name="identifiers">The entities' identifiers.</param>
    /// <typeparam name="TEntity">The type of entity to retrieve.</typeparam>
    Task<TEntity[]> GetAsync(IReadOnlyCollection<int> identifiers);

    /// <summary>
    ///     Adds the provided entity.
    /// </summary>
    /// <param name="newEntity">The new entity to add.</param>
    /// <typeparam name="TEntity">The type of entity to add.</typeparam>
    Task<TEntity> InsertAsync(TEntity newEntity);

    /// <summary>
    ///     Updates an existing entity.
    /// </summary>
    /// <param name="entityToUpdate">The entity to update.</param>
    /// <typeparam name="TEntity">The type of entity to update.</typeparam>
    Task<TEntity> UpdateAsync(TEntity entityToUpdate);

    /// <summary>
    ///     Updates several entities.
    /// </summary>
    /// <param name="entitiesToUpdate">The entities to update.</param>
    /// <typeparam name="TEntity">The type of entity to update.</typeparam>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    Task UpdateRangeAsync(ICollection<TEntity> entitiesToUpdate, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    ///     Retrieve an entity based on its identifier.
    /// </summary>
    /// <param name="id">The entity's identifier.</param>
    /// <typeparam name="TEntity">The type of entity to retrieve.</typeparam>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    Task<TEntity> GetAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves all entities matching the list of provided identifiers.
    /// </summary>
    /// <param name="identifiers">The entities' identifiers.</param>
    /// <typeparam name="TEntity">The type of entity to retrieve.</typeparam>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    Task<TEntity[]> GetAsync(IReadOnlyCollection<int> identifiers, CancellationToken cancellationToken);

    /// <summary>
    ///     Adds the provided entity.
    /// </summary>
    /// <param name="newEntity">The new entity to add.</param>
    /// <typeparam name="TEntity">The type of entity to add.</typeparam>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    Task<TEntity> InsertAsync(TEntity newEntity, CancellationToken cancellationToken);

    /// <summary>
    ///     Adds the provided entities.
    /// </summary>
    /// <param name="newEntities">The new entities to add.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TEntity">The type of entity to add.</typeparam>
    Task<ICollection<TEntity>> InsertAsync(ICollection<TEntity> newEntities, CancellationToken cancellationToken);

    /// <summary>
    ///     Updates an existing entity.
    /// </summary>
    /// <param name="entityToUpdate">The entity to update.</param>
    /// <typeparam name="TEntity">The type of entity to update.</typeparam>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    Task<TEntity> UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken);


    /// <summary>
    ///     Deletes the provided entity, optionally permanently.
    /// </summary>
    /// <param name="entityToRemove">The entity to delete.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <param name="hard"><c>True</c> to hard delete (permanent) the entity, otherwise <c>false</c>.</param>
    /// <returns></returns>
    Task DeleteAsync(TEntity entityToRemove, bool hard = false, CancellationToken cancellationToken = default(CancellationToken));

    Task DeleteAsync(IReadOnlyCollection<int> entitiesToDelete);

    Task DeleteAsync(TEntity[] entitiesToRemove, bool hard, CancellationToken cancellationToken = default(CancellationToken));

    Task DeleteAsync(int id);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
    Task LoadForeignPropertiesAsync(TEntity entity, CancellationToken cancellationToken = default);
}


public abstract class ServiceBase<TContext, TEntity> : IService<TEntity>
    where TEntity : class, IIdentifiable<int>, ISoftDeletable
    where TContext : DbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ServiceBase{TContext,TEntity}" /> class.
    /// </summary>
    protected ServiceBase(TContext dbContext)
    {
        DbContext = dbContext;
    }

    /// <summary>
    ///     The <see cref="Microsoft.EntityFrameworkCore.DbContext" /> instance that can be used to access entities.
    /// </summary>
    public TContext DbContext { get; }

    /// <inheritdoc />
    public Task<TEntity> GetAsync(int id, CancellationToken cancellationToken)
    {
        return DbContext.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public Task<TEntity[]> GetAsync(IReadOnlyCollection<int> identifiers, CancellationToken cancellationToken)
    {
        if (identifiers == null)
        {
            return Task.FromResult(new TEntity[0]);
        }

        return DbContext.Set<TEntity>().Where(e => identifiers.Contains(e.Id)).ToArrayAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TEntity> InsertAsync(TEntity newEntity, CancellationToken cancellationToken)
    {
        DbContext.Set<TEntity>().Add(newEntity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return newEntity;
    }

    public async Task<ICollection<TEntity>> InsertAsync(ICollection<TEntity> newEntities, CancellationToken cancellationToken)
    {
        await DbContext.Set<TEntity>().AddRangeAsync(newEntities, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return newEntities;
    }

    /// <inheritdoc />
    public async Task<TEntity> UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken)
    {
        if (DbContext.Entry(entityToUpdate).State == EntityState.Detached)
        {
            DbContext.Set<TEntity>().Attach(entityToUpdate);
        }

        DbContext.ChangeTracker.DetectChanges();
        await DbContext.SaveChangesAsync(cancellationToken);

        return entityToUpdate;
    }

    public async Task UpdateRangeAsync(ICollection<TEntity> entitiesToUpdate, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (!entitiesToUpdate.Any())
        {
            return;
        }

        foreach (TEntity entity in entitiesToUpdate)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbContext.Set<TEntity>().Attach(entity);
            }
        }

        DbContext.Set<TEntity>().UpdateRange(entitiesToUpdate);

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<TEntity> GetAsync(int id)
    {
        return GetAsync(id, CancellationToken.None);
    }

    public Task<TEntity[]> GetAsync(IReadOnlyCollection<int> identifiers)
    {
        return GetAsync(identifiers, CancellationToken.None);
    }

    public Task<TEntity> InsertAsync(TEntity newEntity)
    {
        return InsertAsync(newEntity, CancellationToken.None);
    }

    public Task<TEntity> UpdateAsync(TEntity entityToUpdate)
    {
        return UpdateAsync(entityToUpdate, CancellationToken.None);
    }
    
    public virtual Task LoadForeignPropertiesAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(IReadOnlyCollection<int> entityKeysToDelete)
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

            //entityToRemove.IsDeleted = true;
        }

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id)
    {
        TEntity entity = DbContext.Set<TEntity>().Find(id);
        if (entity != null)
        {
            await DeleteAsync(entity, false);
        }
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
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