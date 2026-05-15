namespace Ork.Data.Extensions;

public static class AsyncIEnumerableExtensions
{
    /// <summary>
    /// Projects each element of an async-enumerable sequence into a new form by applying
    /// an asynchronous selector function to each member of the source sequence and awaiting
    /// the result.
    /// </summary>
    /// <typeparam name="TSource"> The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements in the result sequence, obtained by running the selector
    /// function for each element in the source sequence and awaiting the result.
    /// </typeparam>
    /// <param name="source">A sequence of elements to invoke a transform function on</param>
    /// <param name="predicate">An asynchronous transform function to apply to each source element</param>
    /// <returns>
    /// An async-enumerable sequence whose elements are the result of invoking the transform
    /// function on each element of the source sequence and awaiting the result
    /// </returns>
    public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, ValueTask<TResult>> predicate)
    {
        return source.ToAsyncEnumerable()
            .Select((TSource x, CancellationToken _) => predicate(x));
    }
}
