namespace Ork.Core.Caching;

/// <summary>
/// Represents a manager for caching between HTTP requests (long term caching)
/// </summary>
public interface IStaticCacheManager : IDisposable, ICacheKeyService
{
    /// <summary>
    /// Get a cached item. If it's not in the cache yet, then load and cache it
    /// </summary>
    Task<T?> GetAsync<T>(CacheKey key, Func<Task<T>> acquire);

    /// <summary>
    /// Get a cached item. If it's not in the cache yet, then load and cache it
    /// </summary>
    Task<T?> GetAsync<T>(CacheKey key, Func<T> acquire);

    /// <summary>
    /// Get a cached item. If it's not in the cache yet, return a default value
    /// </summary>
    Task<T?> GetAsync<T>(CacheKey key, T? defaultValue = default);

    /// <summary>
    /// Get a cached item as an <see cref="object"/> instance, or null on a cache miss.
    /// </summary>
    Task<object?> GetAsync(CacheKey key);

    /// <summary>
    /// Remove the value with the specified key from the cache
    /// </summary>
    Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters);

    /// <summary>
    /// Add the specified key and object to the cache
    /// </summary>
    Task SetAsync<T>(CacheKey key, T data);

    /// <summary>
    /// Remove items by cache key prefix
    /// </summary>
    Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters);

    /// <summary>
    /// Clear all cache data
    /// </summary>
    Task ClearAsync();
}