namespace Ork.Core.Domain.Catalog;

/// <summary>
/// Post copy product event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="originalProduct">Original product</param>
/// <param name="copyProduct">Copy product</param>
public sealed class PostCopyProductEvent(Product originalProduct, Product copyProduct)
{
    /// <summary>
    /// Original product
    /// </summary>
    public Product OriginalProduct { get; } = originalProduct;

    /// <summary>
    /// Copy product
    /// </summary>
    public Product CopyProduct { get; } = copyProduct;
}
