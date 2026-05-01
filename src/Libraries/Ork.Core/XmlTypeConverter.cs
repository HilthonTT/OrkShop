using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace Ork.Core;

/// <summary>
/// Base type converter providing XML serialization/deserialization for a given type.
/// </summary>
public abstract class XmlTypeConverter<T> : TypeConverter where T : class
{
    private static readonly XmlSerializer Serializer = new(typeof(T));

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string valueStr)
        {
            return base.ConvertFrom(context, culture, value);
        }   

        if (string.IsNullOrEmpty(valueStr))
        {
            return null;
        }

        try
        {
            using var reader = new StringReader(valueStr);
            return Serializer.Deserialize(reader) as T;
        }
        catch (InvalidOperationException ex)
        {
            // Log or handle XML deserialization failure
            Trace.TraceWarning($"[{GetType().Name}] XML deserialization failed: {ex.Message}");
            return null;
        }
    }

    public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType != typeof(string))
        {
            return base.ConvertTo(context, culture, value, destinationType)!;
        }

        if (value is not T typedValue)
        {
            return string.Empty;
        }

        using var writer = new StringWriter();
        Serializer.Serialize(writer, typedValue);
        return writer.ToString();
    }
}
