using Ork.Core;
using Ork.Core.Caching;
using System.Linq.Expressions;

namespace Ork.Data;

/// <summary>
/// Represents an entity repository.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Get the entity entry
    /// </summary>
    /// <param name="id">Entity entry identifier</param>
    /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <param name="useShortTermCache">Whether to use short term cache instead of static cache</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the entity entry
    /// </returns>
    Task<TEntity?> GetByIdAsync(
        int? id,
        Func<ICacheKeyService, CacheKey>? getCacheKey = null,
        bool includeDeleted = true,
        bool useShortTermCache = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get entity entries by identifiers
    /// </summary>
    /// <param name="ids">Entity entry identifiers</param>
    /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the entity entries
    /// </returns>
    Task<List<TEntity>> GetByIdsAsync(
        List<int> ids,
        Func<ICacheKeyService, CacheKey>? getCacheKey = null,
        bool includeDeleted = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all entity entries
    /// </summary>
    /// <param name="func">Function to select entries</param>
    /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the entity entries
    /// </returns>
    Task<List<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        Func<ICacheKeyService, CacheKey>? getCacheKey = null,
        bool includeDeleted = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all entity entries
    /// </summary>
    /// <param name="func">Function to select entries</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">Whether to get only the total number of entries without actually loading data</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the paged list of entity entries
    /// </returns>
    Task<PaginationResult<TEntity>> GetAllPagedAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        bool getOnlyTotalCount = false,
        bool includeDeleted = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all entity entries
    /// </summary>
    /// <param name="func">Function to select entries</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="getOnlyTotalCount">Whether to get only the total number of entries without actually loading data</param>
    /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the paged list of entity entries
    /// </returns>
    Task<PaginationResult<TEntity>> GetAllPagedAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        bool getOnlyTotalCount = false,
        bool includeDeleted = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously inserts the specified entity into the data store.
    /// </summary>
    /// <param name="entity">The entity to insert. Cannot be null.</param>
    /// <param name="publishEvent">true to publish an event after insertion; otherwise, false. The default is true.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous insert operation.</returns>
    Task InsertAsync(TEntity entity, bool publishEvent = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously inserts a collection of entities into the data store.
    /// </summary>
    /// <param name="entities">The list of entities to insert. Cannot be null or contain null elements.</param>
    /// <param name="publishEvent">true to publish an event after insertion; otherwise, false. The default is true.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous insert operation.</returns>
    Task InsertAsync(List<TEntity> entities, bool publishEvent = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the specified entity in the data store.
    /// </summary>
    /// <param name="entity">The entity to update. Cannot be null.</param>
    /// <param name="publishEvent">true to publish an event after the update operation completes; otherwise, false. The default is true.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateAsync(TEntity entity, bool publishEvent = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the specified entities in the data store.
    /// </summary>
    /// <param name="entities">The list of entities to update. Cannot be null or empty.</param>
    /// <param name="publishEvent">true to publish an event after the update operation completes; otherwise, false. The default is true.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateAsync(List<TEntity> entities, bool publishEvent = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes the specified entity from the data store.
    /// </summary>
    /// <param name="entity">The entity to delete. Cannot be null.</param>
    /// <param name="publishEvent">true to publish a deletion event after the entity is deleted; otherwise, false. The default is true.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(TEntity entity, bool publishEvent = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes the specified entities from the data store.
    /// </summary>
    /// <param name="entity">The list of entities to delete. Cannot be null or contain null elements.</param>
    /// <param name="publishEvent">true to publish deletion events after removing the entities; otherwise, false. The default is true.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(List<TEntity> entities, bool publishEvent = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes all entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression that defines the conditions of the entities to delete. Only entities matching this predicate will
    /// be removed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of entities deleted.</returns>
    Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously loads the original, unmodified copy of the specified entity from the data store.
    /// </summary>
    /// <param name="entity">The entity instance for which to retrieve the original copy. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. The default value is None.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the original copy of the specified
    /// entity, or null if the entity does not exist in the data store.</returns>
    Task<TEntity?> LoadOriginalCopyAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously removes all records from the underlying data store.
    /// </summary>
    /// <param name="resetIdentity">true to reset identity columns to their seed values after truncation; otherwise, false.</param>
    /// <returns>A task that represents the asynchronous truncate operation.</returns>
    Task TruncateAsync(bool resetIdentity = false);
}
