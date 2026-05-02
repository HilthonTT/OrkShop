using Microsoft.Extensions.Caching.Memory;

namespace Ork.Core.Caching;

public sealed class MemoryCacheLocker(IMemoryCache memoryCache) : ILocker
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    public Task CancelTaskAsync(string key, TimeSpan expirationTime)
    {
        if (_memoryCache.TryGetValue(key, out Lazy<CancellationTokenSource>? tokenSource))
        {
            tokenSource?.Value.Cancel();
        }

        return Task.CompletedTask;
    }

    public Task<bool> IsTaskRunningAsync(string key)
    {
        return Task.FromResult(_memoryCache.TryGetValue(key, out _));
    }

    public Task<bool> PerformActionWithLockAsync(string resource, TimeSpan expirationTime, Func<Task> action)
    {
        return RunAsync(resource, expirationTime, _ => action());
    }

    public async Task RunWithHeartbeatAsync(
        string key, 
        TimeSpan expirationTime, 
        TimeSpan heartbeatInterval, 
        Func<CancellationToken, Task> action, 
        CancellationTokenSource? cancellationTokenSource = null)
    {
        // We ignore expirationTime and heartbeatInterval here, as the cache is not shared with other instances,
        // and will be cleared on system failure anyway. The task is guaranteed to still be running as long as it is in the cache.
        await RunAsync(key, null, action, cancellationTokenSource);
    }

    private async Task<bool> RunAsync(
        string key, 
        TimeSpan? expirationTime, 
        Func<CancellationToken, Task> action, 
        CancellationTokenSource? cancellationTokenSource = default)
    {
        bool started = false;

        try
        {
            var tokenSource = _memoryCache.GetOrCreate(key, entry => new Lazy<CancellationTokenSource>(() =>
            {
                entry.AbsoluteExpirationRelativeToNow = expirationTime;
                entry.SetPriority(CacheItemPriority.NeverRemove);
                started = true;

                return cancellationTokenSource ?? new CancellationTokenSource();
            }, true))?.Value;

            if (tokenSource != null && started)
            {
                await action(tokenSource.Token);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            if (started)
            {
                _memoryCache.Remove(key);
            }
        }

        return started;
    }
}
