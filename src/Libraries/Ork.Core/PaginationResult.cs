namespace Ork.Core;

/// <summary>
/// Represents a single page of a larger result set.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
[Serializable]
public sealed class PaginationResult<T>
{
    private PaginationResult()
    {
    }

    /// <summary>The items on the current page.</summary>
    public IReadOnlyList<T> Items { get; private init; } = [];

    /// <summary>The zero-based index of the current page.</summary>
    public int PageIndex { get; private init; }

    /// <summary>The maximum number of items per page.</summary>
    public int PageSize { get; private init; }

    /// <summary>The total number of items across all pages.</summary>
    public int TotalCount { get; private init; }

    /// <summary>The total number of pages.</summary>
    public int TotalPages => PageSize > 0
        ? (int)Math.Ceiling(TotalCount / (double)PageSize)
        : 0;

    public bool HasPreviousPage => PageIndex > 0;

    public bool HasNextPage => PageIndex < TotalPages - 1;

    /// <summary>
    /// Creates a pagination result from an already-materialized page of items.
    /// </summary>
    public static PaginationResult<T> Create(
        IReadOnlyList<T> items,
        int pageIndex,
        int pageSize,
        int totalCount)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentOutOfRangeException.ThrowIfNegative(pageIndex);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);

        return new PaginationResult<T>
        {
            Items = items,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Creates an empty result, e.g. for a page beyond the available range
    /// or when only the total count was requested.
    /// </summary>
    public static PaginationResult<T> Empty(int pageIndex, int pageSize, int totalCount = 0) =>
        Create([], pageIndex, pageSize, totalCount);
}
