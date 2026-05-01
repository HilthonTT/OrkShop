using Microsoft.EntityFrameworkCore;

namespace Ork.Core;

[Serializable]
public sealed class PaginationResult<T>
{
    private PaginationResult()
    {
    }

    public List<T> Items { get; init; } = [];

    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;

    public static async Task<PaginationResult<T>> CreateAsync(
        IQueryable<T> source, 
        int page, 
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginationResult<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = count
        };
    }
}
