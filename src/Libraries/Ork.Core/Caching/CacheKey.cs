using Ork.Core.Configuration;
using Ork.Core.Infrastructure;

namespace Ork.Core.Caching;

public sealed class CacheKey(string key)
{
    public int CacheTime { get; set; } =
        Singleton<AppSettings>.Instance?.Get<CacheConfig>().DefaultCacheTime
        ?? CacheConfig.Defaults.DefaultCacheTime;

    public string Key { get; set; } = key;

    public CacheKey Create(Func<object, object> createCacheKeyParameters, params object[] keyObjects)
    {
        var cacheKey = new CacheKey(Key);

        if (keyObjects.Length == 0)
        {
            return cacheKey;
        }

        cacheKey.Key = string.Format(cacheKey.Key, keyObjects.Select(createCacheKeyParameters).ToArray());

        return cacheKey;
    }
}
