using Ork.Core.Configuration;
using System.Globalization;
using System.Text;

namespace Ork.Core.Caching;

public abstract class CacheKeyService : ICacheKeyService
{
    protected readonly AppSettings _appSettings;

    public CacheKeyService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    protected string HashAlgorithm => "SHA1";

    protected string PrepareKeyPrefix(string prefix, params object[] prefixParameters)
    {
        return prefixParameters.Length != 0
            ? string.Format(prefix, prefixParameters.Select(CreateCacheKeyParameters).ToArray())
            : prefix;
    }

    protected string CreateIdsHash(IEnumerable<int> ids)
    {
        var identifiers = ids.ToList();

        if (identifiers.Count == 0)
        {
            return string.Empty;
        }

        var identifiersString = string.Join(", ", identifiers.OrderBy(id => id));
        return HashHelper.CreateHash(Encoding.UTF8.GetBytes(identifiersString), HashAlgorithm);
    }

    protected object CreateCacheKeyParameters(object? parameter)
    {
        return parameter switch
        {
            null => "null",
            IEnumerable<int> ids => CreateIdsHash(ids),
            IEnumerable<BaseEntity> entities => CreateIdsHash(entities.Select(entity => entity.Id)),
            BaseEntity entity => entity.Id,
            decimal param => param.ToString(CultureInfo.InvariantCulture),
            _ => parameter
        };
    }

    public CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        return cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);
    }

    public CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

        key.CacheTime = _appSettings.Get<CacheConfig>().DefaultCacheTime;

        return key;
    }
}
