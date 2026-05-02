using System.ComponentModel;
using System.Globalization;

namespace Ork.Core.ComponentModel;

/// <summary>
/// Generic List type converted
/// </summary>
/// <typeparam name="T">Type</typeparam>
public sealed class GenericListTypeConverter<T> : TypeConverter
{
    /// <summary>
    /// Type converter
    /// </summary>
    private readonly TypeConverter typeConverter;

    public GenericListTypeConverter()
    {
        typeConverter = TypeDescriptor.GetConverter(typeof(T))
            ?? throw new InvalidOperationException("No type converter exists for type " + typeof(T).FullName);
    }

    /// <summary>
    /// Get string array from a comma-separate string
    /// </summary>
    /// <param name="input">Input</param>
    /// <returns>Array</returns>
    public string[] GetStringArray(string? input)
    {
        return string.IsNullOrEmpty(input) ? [] : input.Split(',').Select(x => x.Trim()).ToArray();
    }

    /// <summary>
    /// Gets a value indicating whether this converter can        
    /// convert an object in the given source type to the native type of the converter
    /// using the context.
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="sourceType">Source type</param>
    /// <returns>Result</returns>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        if (sourceType != typeof(string))
        {
            return base.CanConvertFrom(context, sourceType);
        }

        var items = GetStringArray(sourceType.ToString());

        return items.Length != 0;
    }

    /// <summary>
    /// Converts the given object to the converter's native type.
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="culture">Culture</param>
    /// <param name="value">Value</param>
    /// <returns>Result</returns>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value is not string && value is not null)
        {
            return base.ConvertFrom(context, culture, value);
        }

        string[] items = GetStringArray((string?)value);

        return items.Select(typeConverter.ConvertFromInvariantString)
            .Where(item => item is not null)
            .Cast<T>()
            .ToList();
    }

    /// <summary>
    /// Converts the given value object to the specified destination type using the specified context and arguments
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="culture">Culture</param>
    /// <param name="value">Value</param>
    /// <param name="destinationType">Destination type</param>
    /// <returns>Result</returns>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType != typeof(string))
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        var result = string.Empty;

        if (value is null)
        {
            return result;
        }

        var cultureInvariantStrings = ((IList<T>)value)
            .Select(o => Convert.ToString(o, CultureInfo.InvariantCulture));

        result = string.Join(',', cultureInvariantStrings);

        return result;
    }
}