using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Ork.Core.Configuration;

namespace Ork.Core.Caching;

public class MemoryCacheManager(AppSettings appSettings, IMemoryCache memoryCache, ICacheKeyManager cacheKeyManager) 
    : CacheKeyService(appSettings), IStaticCacheManager
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ICacheKeyManager _cacheKeyManager = cacheKeyManager;

    private bool _disposed;
    private CancellationTokenSource _clearToken = new();

    public async Task ClearAsync()
    {
        await _clearToken.CancelAsync();
        _clearToken.Dispose();
        _clearToken = new CancellationTokenSource();
        _cacheKeyManager.Clear();

        if (_memoryCache is ISynchronizedMemoryCache cache)
        {
            await cache.ClearCacheAsync();
        }
    }

    public async Task<T?> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        if ((key?.CacheTime ?? 0) <= 0 || string.IsNullOrWhiteSpace(key?.Key))
        {
            return await acquire();
        }   

        var task = _memoryCache.GetOrCreate(
            key.Key,
            entry =>
            {
                entry.SetOptions(PrepareEntryOptions(key));
                return new Lazy<Task<T>>(acquire, true);
            });

        try
        {
            var data = await task!.Value;

            //if a cached function return null, remove it from the cache
            if (data is null)
            {
                await RemoveAsync(key);
            }

            return data;
        }
        catch (Exception ex)
        {
            //if a cached function throws an exception, remove it from the cache
            await RemoveAsync(key);

            if (ex is NullReferenceException)
            {
                return default;
            }

            throw;
        }
    }

    public Task<T?> GetAsync<T>(CacheKey key, Func<T> acquire)
    {
        return GetAsync(key, () => Task.FromResult(acquire()));
    }

    public async Task<T?> GetAsync<T>(CacheKey key, T? defaultValue = default)
    {
        var value = _memoryCache.Get<Lazy<Task<T>>>(key.Key)?.Value;

        try
        {
            return value != null ? await value : defaultValue;
        }
        catch
        {
            //if a cached function throws an exception, remove it from the cache
            await RemoveAsync(key);

            throw;
        }
    }

    public async Task<object?> GetAsync(CacheKey key)
    {
        object? entry = _memoryCache.Get(key.Key);
        if (entry is null)
        {
            return null;
        }

        try
        {
            if (entry.GetType().GetProperty("Value")?.GetValue(entry) is not Task task)
            {
                return null;
            }

            await task;

            return task.GetType().GetProperty("Result")!.GetValue(task);
        }
        catch
        {
            //if a cached function throws an exception, remove it from the cache
            await RemoveAsync(key);

            throw;
        }
    }

    public Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        string key = PrepareKey(cacheKey, cacheKeyParameters).Key;

        _memoryCache.Remove(key);
        _cacheKeyManager.RemoveKey(key);

        return Task.CompletedTask;
    }

    public async Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
    {
        string deletePrefix = PrepareKeyPrefix(prefix, prefixParameters);

        foreach (var key in _cacheKeyManager.RemoveByPrefix(deletePrefix))
        {
            _memoryCache.Remove(key);
        }

        if (_memoryCache is ISynchronizedMemoryCache cache)
        {
            await cache.RemoveByPrefixAsync(deletePrefix);
        }
    }

    public Task SetAsync<T>(CacheKey key, T data)
    {
        if (data is null || (key?.CacheTime ?? 0) <= 0 || string.IsNullOrWhiteSpace(key?.Key))
        {
            return Task.CompletedTask;
        }


        _memoryCache.Set(
            key.Key,
            new Lazy<Task<T>>(() => Task.FromResult(data), true),
            PrepareEntryOptions(key));

        return Task.CompletedTask;
    }

    public void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // don't dispose of the MemoryCache, as it is injected
            _clearToken.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// The callback method which gets called when a cache entry expires.
    /// </summary>
    /// <param name="key">The key of the entry being evicted.</param>
    /// <param name="value">The value of the entry being evicted.</param>
    /// <param name="reason">The <see cref="EvictionReason"/>.</param>
    /// <param name="state">The information that was passed when registering the callback.</param>
    private void OnEviction(object key, object? value, EvictionReason reason, object? state)
    {
        switch (reason)
        {
            // we clean up after ourselves elsewhere
            case EvictionReason.Removed:
            case EvictionReason.Replaced:
            case EvictionReason.TokenExpired:
                break;
            //if the entry was evicted by the cache itself, we remove the key
            default:
                //checks if the eviction callback happens after the item is re-added to the cache to prevent the erroneously removing entry from the key manager
                if (!_memoryCache.TryGetValue(key, out _))
                {
                    _cacheKeyManager.RemoveKey(key as string ?? "");
                }
                break;
        }
    }

    private MemoryCacheEntryOptions PrepareEntryOptions(CacheKey key)
    {
        //set expiration time for the passed cache key
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(key.CacheTime)
        };

        //add token to clear cache entries
        options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
        options.RegisterPostEvictionCallback(OnEviction);
        _cacheKeyManager.AddKey(key.Key);

        return options;
    }
}
