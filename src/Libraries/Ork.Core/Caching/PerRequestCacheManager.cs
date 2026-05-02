using Ork.Core.Configuration;
using Ork.Core.Infrastructure;

namespace Ork.Core.Caching;

public sealed class PerRequestCacheManager(AppSettings appSettings) 
    : CacheKeyService(appSettings), IShortTermCacheManager
{
    private readonly ConcurrentTrie<object> _concurrentCollection = new();

    public async Task<T> GetAsync<T>(Func<Task<T>> acquire, CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters).Key;

        if (_concurrentCollection.TryGetValue(key, out var data))
        {
            return (T)data;
        }

        var result = await acquire();

        if (result is not null)
        {
            _concurrentCollection.Add(key, result);
        }

        return result;
    }

    public void Remove(string cacheKey, params object[] cacheKeyParameters)
    {
        _concurrentCollection.Remove(PrepareKey(new CacheKey(cacheKey), cacheKeyParameters).Key);
    }

    public void RemoveByPrefix(string prefix, params object[] prefixParameters)
    {
        var keyPrefix = PrepareKeyPrefix(prefix, prefixParameters);
        _concurrentCollection.Prune(keyPrefix, out _);
    }
}
