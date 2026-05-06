using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Ork.Core.Http.Extensions;

public static class SessionExtensions
{
    public static async Task LoadAsync(ISession session, CancellationToken cancellationToken = default)
    {
        try
        {
            await session.LoadAsync(cancellationToken);
        }
        catch (Exception)
        {
            // fallback to synchronous handling
        }
    }

    public static async Task SetAsync<T>(
        this ISession session, 
        string key, 
        T value, 
        CancellationToken cancellationToken = default)
    {
        await LoadAsync(session, cancellationToken);
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static async Task<T?> GetAsync<T>(
        this ISession session, 
        string key, 
        CancellationToken cancellationToken = default)
    {
        await LoadAsync(session, cancellationToken);
        var value = session.GetString(key);
        return value is null ? default : JsonConvert.DeserializeObject<T>(value);
    }

    public static async Task RemoveAsync(
        this ISession session, 
        string key, 
        CancellationToken cancellationToken = default)
    {
        await LoadAsync(session, cancellationToken);
        await session.RemoveAsync(key, cancellationToken);
    }

    public static async Task ClearAsync(
        this ISession session,
        CancellationToken cancellationToken = default)
    {
        await LoadAsync(session, cancellationToken);
        await session.ClearAsync(cancellationToken);
    }
}
