using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Net;

namespace Ork.Core.Http.Extensions;

public static class HttpRequestExtensions
{
    public static bool IsPostRequest(this HttpRequest request)
    {
        return request.Method.Equals(WebRequestMethods.Http.Post, StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool IsGetRequest(this HttpRequest request)
    {
        return request.Method.Equals(WebRequestMethods.Http.Get, StringComparison.InvariantCultureIgnoreCase);
    }

    public static async Task<StringValues> GetFormValueAsync(
        this HttpRequest request,
        string formKey,
        CancellationToken cancellationToken = default)
    {
        if (!request.HasFormContentType)
        {
            return new StringValues();
        }

        IFormCollection form = await request.ReadFormAsync(cancellationToken);
        return form[formKey];
    }

    public static async Task<bool> IsFormKeyExistsAsync(
        this HttpRequest request, 
        string formKey, 
        CancellationToken cancellationToken = default)
    {
        return await IsFormAnyAsync(request, key => key.Equals(formKey), cancellationToken);
    }

    public static async Task<bool> IsFormAnyAsync(
        this HttpRequest request, 
        Func<string, bool>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        if (!request.HasFormContentType)
        {
            return false;
        }

        IFormCollection form = await request.ReadFormAsync(cancellationToken);

        return predicate is null ?
            form.Count != 0 :
            form.Keys.Any(predicate);
    }

    public static async Task<(bool KeyExists, StringValues FormValue)> TryGetFormValueAsync(
        this HttpRequest request, 
        string formKey,
        CancellationToken cancellationToken = default)
    {
        if (!request.HasFormContentType)
        {
            return (false, default);
        }

        IFormCollection form = await request.ReadFormAsync(cancellationToken);
        bool flag = form.TryGetValue(formKey, out StringValues value);

        return (flag, value);
    }

    public static async Task<IFormFile?> GetFirstOrDefaultFileAsync(this HttpRequest request)
    {
        if (!request.HasFormContentType)
        {
            return default;
        }

        IFormCollection form = await request.ReadFormAsync();
        IFormFileCollection files = form.Files;

        return files.Count > 0 ? files[0] : default;
    }
}
