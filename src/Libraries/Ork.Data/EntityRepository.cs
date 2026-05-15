using LinqToDB.Async;
using Ork.Core;
using Ork.Core.Caching;
using Ork.Core.Configuration;
using Ork.Core.Domain.Common;
using Ork.Core.Events;
using Ork.Data.Extensions;
using System.Linq.Expressions;
using System.Transactions;

namespace Ork.Data;

public sealed class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    // Resolved once per closed generic type instead of reflecting on every call.
    private static readonly bool IsSoftDeletedEntity =
        typeof(ISoftDeletedEntity).IsAssignableFrom(typeof(TEntity));

    private readonly IOrkDataProvider _dataProvider;
    private readonly IEventPublisher _eventPublisher;
    private readonly IShortTermCacheManager _shortTermCacheManager;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly bool _usingDistributedCache;

    public EntityRepository(
        IOrkDataProvider dataProvider,
        IEventPublisher eventPublisher,
        IShortTermCacheManager shortTermCacheManager,
        IStaticCacheManager staticCacheManager,
        AppSettings appSettings)
    {
        _dataProvider = dataProvider;
        _eventPublisher = eventPublisher;
        _shortTermCacheManager = shortTermCacheManager;
        _staticCacheManager = staticCacheManager;
        _usingDistributedCache = appSettings.Get<DistributedCacheConfig>().DistributedCacheType switch
        {
            DistributedCacheType.Redis => true,
            DistributedCacheType.SqlServer => true,
            _ => false
        };
    }

    public IQueryable<TEntity> Table => _dataProvider.GetTable<TEntity>();

    public async Task DeleteAsync(TEntity entity, bool publishEvent = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        switch (entity)
        {
            case ISoftDeletedEntity softDeletedEntity:
                softDeletedEntity.Deleted = true;
                await _dataProvider.UpdateEntityAsync(entity, cancellationToken);
                break;

            default:
                await _dataProvider.DeleteEntityAsync(entity, cancellationToken);
                break;
        }

        if (publishEvent)
        {
            await _eventPublisher.EntityDeletedAsync(entity, cancellationToken);
        }
    }

    public async Task DeleteAsync(List<TEntity> entities, bool publishEvent = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (entities.Count == 0)
        {
            return;
        }

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            if (IsSoftDeletedEntity)
            {
                foreach (var entity in entities)
                {
                    ((ISoftDeletedEntity)entity).Deleted = true;
                }

                await _dataProvider.UpdateEntitiesAsync(entities, cancellationToken);
            }
            else
            {
                await _dataProvider.BulkDeleteEntitiesAsync(entities, cancellationToken);
            }

            transaction.Complete();
        }

        if (publishEvent)
        {
            foreach (var entity in entities)
            {
                await _eventPublisher.EntityDeletedAsync(entity, cancellationToken);
            }
        }
    }

    public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var countDeletedRecords = await _dataProvider.BulkDeleteEntitiesAsync(predicate, cancellationToken);
        transaction.Complete();

        return countDeletedRecords;
    }

    public async Task<List<TEntity>> GetAllAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        Func<ICacheKeyService, CacheKey>? getCacheKey = null,
        bool includeDeleted = true,
        CancellationToken cancellationToken = default)
    {
        async Task<List<TEntity>> getAllAsync()
        {
            var query = AddDeletedFilter(Table, includeDeleted);
            query = func is not null ? await func(query) : query;

            return await query.ToListAsync(cancellationToken);
        }

        return await GetEntitiesAsync(getAllAsync, getCacheKey);
    }

    public Task<PaginationResult<TEntity>> GetAllPagedAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        bool getOnlyTotalCount = false,
        bool includeDeleted = true,
        CancellationToken cancellationToken = default)
    {
        return GetAllPagedAsync(
            func is null ? null : query => Task.FromResult(func(query)),
            pageIndex,
            pageSize,
            getOnlyTotalCount,
            includeDeleted,
            cancellationToken);
    }

    public async Task<PaginationResult<TEntity>> GetAllPagedAsync(
        Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        bool getOnlyTotalCount = false,
        bool includeDeleted = true,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(pageIndex);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

        var query = AddDeletedFilter(Table, includeDeleted);

        if (func is not null)
        {
            query = await func(query);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        // long avoids int overflow when pageSize is int.MaxValue.
        var skip = (long)pageIndex * pageSize;

        if (getOnlyTotalCount || skip >= totalCount)
        {
            return PaginationResult<TEntity>.Empty(pageIndex, pageSize, totalCount);
        }

        var items = await query
            .Skip((int)skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return PaginationResult<TEntity>.Create(items, pageIndex, pageSize, totalCount);
    }

    public async Task<TEntity?> GetByIdAsync(
        int? id,
        Func<ICacheKeyService, CacheKey>? getCacheKey = null,
        bool includeDeleted = true,
        bool useShortTermCache = false,
        CancellationToken cancellationToken = default)
    {
        if (!id.HasValue || id.Value == 0)
        {
            return null;
        }

        var entityId = id.Value;

        async Task<TEntity?> getEntityAsync()
        {
            return await AddDeletedFilter(Table, includeDeleted)
                .FirstOrDefaultAsync(entity => entity.Id == entityId, cancellationToken);
        }

        if (getCacheKey is null)
        {
            return await getEntityAsync();
        }

        ICacheKeyService cacheKeyService = useShortTermCache ? _shortTermCacheManager : _staticCacheManager;

        //caching
        CacheKey cacheKey = getCacheKey(cacheKeyService)
                    ?? cacheKeyService.PrepareKeyForDefaultCache(OrkEntityCacheDefaults<TEntity>.ByIdCacheKey, entityId);

        if (useShortTermCache)
        {
            return await _shortTermCacheManager.GetAsync(getEntityAsync, cacheKey);
        }

        return await _staticCacheManager.GetAsync(cacheKey, getEntityAsync);
    }

    public async Task<List<TEntity>> GetByIdsAsync(
        List<int> ids,
        Func<ICacheKeyService, CacheKey>? getCacheKey = null,
        bool includeDeleted = true,
        CancellationToken cancellationToken = default)
    {
        if (ids.Count == 0)
        {
            return [];
        }

        static List<TEntity> sortByIdList(IList<int> listOfId, IDictionary<int, TEntity> entitiesById)
        {
            var sortedEntities = new List<TEntity>(listOfId.Count);

            foreach (var id in listOfId)
            {
                if (entitiesById.TryGetValue(id, out var entry))
                {
                    sortedEntities.Add(entry);
                }
            }

            return sortedEntities;
        }

        async Task<List<TEntity>> getByIdsAsync(IList<int> listOfId, bool sort = true)
        {
            var query = AddDeletedFilter(Table, includeDeleted)
                .Where(entry => listOfId.Contains(entry.Id));

            return sort
                ? sortByIdList(listOfId, await query.ToDictionaryAsync(entry => entry.Id, cancellationToken))
                : await query.ToListAsync(cancellationToken);
        }

        if (getCacheKey is null)
        {
            return await getByIdsAsync(ids);
        }

        //caching
        var cacheKey = getCacheKey(_staticCacheManager);
        if (cacheKey is null && _usingDistributedCache)
        {
            cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(OrkEntityCacheDefaults<TEntity>.ByIdsCacheKey, ids);
        }
        if (cacheKey is not null)
        {
            return await _staticCacheManager.GetAsync(cacheKey, () => getByIdsAsync(ids)) ?? [];
        }

        //if we are using an in-memory cache, we can optimize by caching each entity individually for maximum reusability.
        //with a distributed cache, the overhead would be too high.
        Dictionary<int, TEntity> cachedById = await ids
            .Distinct()
            .SelectAwait(async id => await _staticCacheManager.GetAsync(
                _staticCacheManager.PrepareKeyForDefaultCache(OrkEntityCacheDefaults<TEntity>.ByIdCacheKey, id),
                default(TEntity)))
            .Where(entity => entity is not null)
            .ToDictionaryAsync(entity => entity!.Id, entity => entity!, cancellationToken: cancellationToken) ?? [];

        var missingIds = ids.Except(cachedById.Keys).ToList();

        var missingEntities = missingIds.Count > 0 ? await getByIdsAsync(missingIds, false) : [];

        foreach (var entity in missingEntities)
        {
            await _staticCacheManager.SetAsync(_staticCacheManager.PrepareKeyForDefaultCache(OrkEntityCacheDefaults<TEntity>.ByIdCacheKey, entity.Id), entity);
            cachedById[entity.Id] = entity;
        }

        return sortByIdList(ids, cachedById);
    }

    public async Task InsertAsync(TEntity entity, bool publishEvent = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        // NOTE: 'InserEntityAsync' looks like a typo for 'InsertEntityAsync' on IOrkDataProvider.
        await _dataProvider.InserEntityAsync(entity, cancellationToken);

        if (publishEvent)
        {
            await _eventPublisher.EntityInsertedAsync(entity, cancellationToken);
        }
    }

    public async Task InsertAsync(
        List<TEntity> entities,
        bool publishEvent = true,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (entities.Count == 0)
        {
            return;
        }

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _dataProvider.BulkInsertEntitiesAsync(entities, cancellationToken);
            transaction.Complete();
        }

        if (publishEvent)
        {
            foreach (var entity in entities)
            {
                await _eventPublisher.EntityInsertedAsync(entity, cancellationToken);
            }
        }
    }

    public Task<TEntity?> LoadOriginalCopyAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return Table.FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken);
    }

    public Task TruncateAsync(bool resetIdentity = false)
    {
        return _dataProvider.TruncateAsync<TEntity>(resetIdentity);
    }

    public async Task UpdateAsync(TEntity entity, bool publishEvent = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _dataProvider.UpdateEntityAsync(entity, cancellationToken);

        if (publishEvent)
        {
            await _eventPublisher.EntityUpdatedAsync(entity, cancellationToken);
        }
    }

    public async Task UpdateAsync(List<TEntity> entities, bool publishEvent = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (entities.Count == 0)
        {
            return;
        }

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _dataProvider.UpdateEntitiesAsync(entities, cancellationToken);
            transaction.Complete();
        }

        if (publishEvent)
        {
            foreach (var entity in entities)
            {
                await _eventPublisher.EntityUpdatedAsync(entity, cancellationToken);
            }
        }
    }

    private async Task<List<TEntity>> GetEntitiesAsync(
        Func<Task<List<TEntity>>> getAllAsync,
        Func<IStaticCacheManager, CacheKey>? getCacheKey)
    {
        if (getCacheKey is null)
        {
            return await getAllAsync();
        }

        //caching
        CacheKey cacheKey = getCacheKey(_staticCacheManager)
                ?? _staticCacheManager.PrepareKeyForDefaultCache(OrkEntityCacheDefaults<TEntity>.AllCacheKey);

        return await _staticCacheManager.GetAsync(cacheKey, getAllAsync) ?? [];
    }

    private static IQueryable<TEntity> AddDeletedFilter(IQueryable<TEntity> query, bool includeDeleted)
    {
        if (includeDeleted || !IsSoftDeletedEntity)
        {
            return query;
        }

        return query.OfType<ISoftDeletedEntity>().Where(entry => !entry.Deleted).OfType<TEntity>();
    }
}
