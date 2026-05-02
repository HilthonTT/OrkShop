using Ork.Core.Infrastructure;

namespace Ork.Core.Caching;

/// <summary>
/// Cache key manager
/// </summary>
/// <remarks>
/// This class should be registered on IoC as singleton instance
/// </remarks>
public sealed class CacheKeyManager : ICacheKeyManager
{
    private readonly IConcurrentCollection<byte> _keys;

    public CacheKeyManager(IConcurrentCollection<byte> keys)
    {
        _keys = keys;
    }

    public IEnumerable<string> Keys => _keys.Keys;

    /// <summary>
    /// Add the key
    /// </summary>
    /// <param name="key">The key to add</param>
    public void AddKey(string key)
    {
        _keys.Add(key, default);
    }

    /// <summary>
    /// Remove all keys
    /// </summary>
    public void Clear()
    {
        _keys.Clear();
    }

    /// <summary>
    /// Remove keys by prefix
    /// </summary>
    /// <param name="prefix">Prefix to delete keys</param>
    /// <returns>The list of removed keys</returns>
    public IEnumerable<string> RemoveByPrefix(string prefix)
    {
        if (_keys.Prune(prefix, out IConcurrentCollection<byte> subCollection) || subCollection.Keys is not null)
        {
            return subCollection.Keys;
        }

        return [];
    }

    /// <summary>
    /// Remove the key
    /// </summary>
    /// <param name="key">The key to remove</param>
    public void RemoveKey(string key)
    {
        _keys.Remove(key);
    }
}
