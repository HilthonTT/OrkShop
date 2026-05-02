using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Ork.Core.Caching;

public sealed class DistributedCacheLocker : ILocker
{
    private static readonly string _running = JsonConvert.SerializeObject(TaskStatus.Running);
    private readonly IDistributedCache _distributedCache;

    public DistributedCacheLocker(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task CancelTaskAsync(string key, TimeSpan expirationTime)
    {
        string? status = await _distributedCache.GetStringAsync(key);
        
        if (!string.IsNullOrWhiteSpace(status) && JsonConvert.DeserializeObject<TaskStatus>(status) != TaskStatus.Canceled)
        {
            await _distributedCache.SetStringAsync(
                 key,
                 JsonConvert.SerializeObject(TaskStatus.Canceled),
                 new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expirationTime });
        }
    }

    public async Task<bool> IsTaskRunningAsync(string key)
    {
        string? taskKey = await _distributedCache.GetStringAsync(key);
        return !string.IsNullOrEmpty(taskKey);
    }

    public async Task<bool> PerformActionWithLockAsync(string resource, TimeSpan expirationTime, Func<Task> action)
    {
        // ensure that lock is acquired
        string? strLock = await _distributedCache.GetStringAsync(resource);
        if (!string.IsNullOrWhiteSpace(strLock))
        {
            return false;
        }

        try
        {
            await _distributedCache.SetStringAsync(resource, resource, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            });

            await action();

            return true;
        }
        finally
        {
            //release lock even if action fails
            await _distributedCache.RemoveAsync(resource);
        }
    }

    public async Task RunWithHeartbeatAsync(
        string key, 
        TimeSpan expirationTime, 
        TimeSpan heartbeatInterval, 
        Func<CancellationToken, Task> action, 
        CancellationTokenSource? cancellationTokenSource = null)
    {
        // ensure that lock is acquired
        string? hearbeat = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrWhiteSpace(hearbeat))
        {
            return;
        }

        var tokenSource = cancellationTokenSource ?? new CancellationTokenSource();

        try
        {
            // run heartbeat early to minimize risk of multiple execution
            await _distributedCache.SetStringAsync(
                key,
                _running,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expirationTime },
                token: tokenSource.Token);

            await using var timer = new Timer(
                callback: _ =>
                {
                    try
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        var status = _distributedCache.GetString(key);
                        if (!string.IsNullOrEmpty(status) && JsonConvert.DeserializeObject<TaskStatus>(status) ==
                            TaskStatus.Canceled)
                        {
                            tokenSource.Cancel();
                            return;
                        }

                        _distributedCache.SetString(
                            key,
                            _running,
                            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expirationTime });
                    }
                    catch (OperationCanceledException) { }
                },
                state: null,
                dueTime: 0,
                period: (int)heartbeatInterval.TotalMilliseconds);

            await action(tokenSource.Token);
        }
        catch (OperationCanceledException) { }
        finally
        {
            await _distributedCache.RemoveAsync(key);
        }
    }
}
