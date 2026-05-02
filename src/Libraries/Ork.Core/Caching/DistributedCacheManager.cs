using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Ork.Core.Configuration;
using Ork.Core.Infrastructure;
using System.Collections.Concurrent;

namespace Ork.Core.Caching;

public abstract class DistributedCacheManager(AppSettings appSettings,
    IDistributedCache distributedCache,
    ICacheKeyManager cacheKeyManager,
    IConcurrentCollection<object> concurrentCollection) : CacheKeyService(appSettings), IStaticCacheManager
{
    #region Fields

    protected readonly ICacheKeyManager _localKeyManager = cacheKeyManager;
    protected readonly IDistributedCache _distributedCache = distributedCache;
    protected readonly IConcurrentCollection<object> _concurrentCollection = concurrentCollection;
    protected readonly ConcurrentDictionary<string, Lazy<Task<object?>>> _ongoing = new();

    #endregion

    #region Utilities

    protected virtual void ClearInstanceData()
    {
        _concurrentCollection.Clear();
        _localKeyManager.Clear();
    }

    protected virtual IEnumerable<string> RemoveByPrefixInstanceData(string prefix, params object[] prefixParameters)
    {
        var keyPrefix = PrepareKeyPrefix(prefix, prefixParameters);
        _concurrentCollection.Prune(keyPrefix, out _);

        return _localKeyManager.RemoveByPrefix(keyPrefix);
    }

    protected virtual DistributedCacheEntryOptions PrepareEntryOptions(CacheKey key)
    {
        return new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(key.CacheTime)
        };
    }

    protected virtual void SetLocal(string key, object value)
    {
        _concurrentCollection.Add(key, value);
        _localKeyManager.AddKey(key);
    }

    protected virtual void RemoveLocal(string key)
    {
        _concurrentCollection.Remove(key);
        _localKeyManager.RemoveKey(key);
    }

    // Return T? to correctly express that deserialization or a cache miss can yield null
    protected virtual async Task<(bool isSet, T? item)> TryGetItemAsync<T>(string key)
    {
        var json = await _distributedCache.GetStringAsync(key);

        return string.IsNullOrEmpty(json)
            ? (false, default)
            : (true, JsonConvert.DeserializeObject<T>(json));
    }

    protected virtual async Task RemoveAsync(string key, bool removeFromInstance = true)
    {
        _ongoing.TryRemove(key, out _);
        await _distributedCache.RemoveAsync(key);

        if (!removeFromInstance)
        {
            return;
        }

        RemoveLocal(key);
    }

    #endregion

    #region Methods

    public virtual async Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        await RemoveAsync(PrepareKey(cacheKey, cacheKeyParameters).Key);
    }

    public virtual async Task<T?> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        if (_concurrentCollection.TryGetValue(key.Key, out var data))
        {
            return (T?)data;  // data is object? — safe cast, null propagates naturally
        }

        var lazy = _ongoing.GetOrAdd(key.Key, _ => new(async () => await acquire(), true));
        var setTask = Task.CompletedTask;

        try
        {
            if (lazy.IsValueCreated)
            {
                return (T?)await lazy.Value;
            }

            var (isSet, item) = await TryGetItemAsync<T>(key.Key);
            if (!isSet)
            {
                item = (T?)await lazy.Value;

                if (key.CacheTime == 0 || item is null)
                {
                    return item;
                }

                setTask = _distributedCache.SetStringAsync(
                    key.Key,
                    JsonConvert.SerializeObject(item),
                    PrepareEntryOptions(key));
            }

            // Only call SetLocal when we have a concrete non-null value to store
            if (item is not null)
            {
                SetLocal(key.Key, item);
            }

            return item;
        }
        finally
        {
            _ = setTask.ContinueWith(_ =>
                _ongoing.TryRemove(new KeyValuePair<string, Lazy<Task<object?>>>(key.Key, lazy)));
        }
    }

    public virtual async Task<T?> GetAsync<T>(CacheKey key, Func<T> acquire)
    {
        return await GetAsync(key, () => Task.FromResult(acquire()));
    }

    // Match the interface's T? defaultValue signature to avoid nullability mismatch
    public virtual async Task<T?> GetAsync<T>(CacheKey key, T? defaultValue = default)
    {
        var value = await _distributedCache.GetStringAsync(key.Key);

        return value is not null
            ? JsonConvert.DeserializeObject<T>(value)
            : defaultValue;
    }

    public virtual async Task<object?> GetAsync(CacheKey key)
    {
        return await GetAsync<object>(key);
    }

    public virtual async Task SetAsync<T>(CacheKey key, T data)
    {
        // Guard null data and zero/negative cache time before ever touching key.Key
        if (data is null || key.CacheTime <= 0)
        {
            return;
        }

        var lazy = new Lazy<Task<object?>>(() => Task.FromResult(data as object ?? null), true);

        try
        {
            _ongoing.TryAdd(key.Key, lazy);
            var resolved = await lazy.Value;

            if (resolved is not null)
                SetLocal(key.Key, resolved);

            await _distributedCache.SetStringAsync(
                key.Key,
                JsonConvert.SerializeObject(data),
                PrepareEntryOptions(key));
        }
        finally
        {
            _ongoing.TryRemove(new KeyValuePair<string, Lazy<Task<object?>>>(key.Key, lazy));
        }
    }

    public abstract Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters);

    public abstract Task ClearAsync();

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    #endregion
}
