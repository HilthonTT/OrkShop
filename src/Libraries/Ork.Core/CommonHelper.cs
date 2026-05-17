using Ork.Core.Infrastructure;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Ork.Core;

public partial class CommonHelper
{
    // we use regular expression based on RFC 5322 Official Standard(see https://emailregex.com/)
    private const string EmailExpression = 
        @"^(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])$";

    /// <returns>Regular expression</returns>
    [GeneratedRegex(EmailExpression, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture, "en-US")]
    public static partial Regex GetEmailRegex();

    public static IOrkFileProvider DefaultFileProvider { get; set; } = default!;

    public static string EnsureSuscriberEmailOrThrow(string? email)
    {
        string output = EnsureNotNull(email);
        output = output.Trim();
        output = EnsureMaximumLength(output, 255);

        if (!IsValidEmail(output))
        {
            throw new OrkException("Email is not valid.");
        }

        return output;
    }

    public static string EnsureMaximumLength(string str, int maxLength, string? postfix = null)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        if (str.Length <= maxLength)
        {
            return str;
        }

        int pLen = postfix?.Length ?? 0;

        string result = str[0..(maxLength - pLen)];
        if (!string.IsNullOrEmpty(postfix))
        {
            result += postfix;
        }

        return result;
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        email = email.Trim();

        return GetEmailRegex().IsMatch(email);
    }

    public static string EnsureNumericOnly(string str)
    {
        return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(char.IsDigit).ToArray());
    }

    public static string EnsureNotNull(string? str)
    {
        return string.IsNullOrWhiteSpace(str) ? string.Empty : str;
    }

    public static bool AreNullOrEmpty(params string[] stringsToValidate)
    {
        return stringsToValidate.Any(string.IsNullOrWhiteSpace);
    }

    public static bool ArraysEqual<T>(T[] a1, T[] a2)
    {
        if (ReferenceEquals(a1, a2))
        {
            return true;
        }

        if (a1.Length != a2.Length)
        {
            return false;
        }

        if (a1 is null || a2 is null)
        {
            return false;
        }

        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        return !a1.Where((t, i) => !comparer.Equals(t, a2[i])).Any();
    }

    public static void SetProperty(object instance, string propertyName, object? value)
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));
        ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));

        var instanceType = instance.GetType();
        var pi = instanceType.GetProperty(propertyName)
            ?? throw new OrkException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);

        if (!pi.CanWrite)
        {
            throw new OrkException(
                "The property '{0}' on the instance of type '{1}' does not have a setter.", 
                propertyName, instanceType);
        }

        if (value is not null && !value.GetType().IsAssignableFrom(pi.PropertyType))
        {
            value = To(value, pi.PropertyType);
        }

        pi.SetValue(instance, value, []);
    }

    public static object? To(object value, Type destinationType, CultureInfo culture)
    {
        if (value is null)
        {
            return null;
        }

        var sourceType = value.GetType();

        var destinationConverter = TypeDescriptor.GetConverter(destinationType);
        if (destinationConverter.CanConvertFrom(sourceType))
        {
            return destinationConverter.ConvertFrom(null, culture, value);
        }

        var sourceConverter = TypeDescriptor.GetConverter(sourceType);
        if (sourceConverter.CanConvertTo(destinationType))
        {
            return sourceConverter.ConvertTo(null, culture, value, destinationType);
        }

        if (destinationType.IsEnum && value is int v)
        {
            return Enum.ToObject(destinationType, v);
        }

        if (!destinationType.IsInstanceOfType(value))
        {
            return Convert.ChangeType(value, destinationType, culture);
        }

        return value;
    }

    public static object? To(object value, Type destinationType)
    {
        return To(value, destinationType, CultureInfo.InvariantCulture);
    }

    public static T? To<T>(object value)
    {
        //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        return (T?)To(value, typeof(T));
    }

    public static string SplitCamelCaseWord(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }

        var result = str.ToCharArray()
            .Select(p => p.ToString())
            .Aggregate(string.Empty, (current, c) => current + (c == c.ToUpperInvariant() ? $" {c}" : c));

        //ensure no spaces (e.g. when the first letter is upper case)
        result = result.TrimStart();

        return result;
    }

    public static string SnakeCaseToPascalCase(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }

        var tempString = str.ToLower().Replace("_", " ");

        return CultureInfo.InvariantCulture.TextInfo
            .ToTitleCase(tempString)
            .Replace(" ", string.Empty);
    }

    public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
    {
        //source: http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
        //this assumes you are looking for the western idea of age and not using East Asian reckoning.
        var age = endDate.Year - startDate.Year;
        if (startDate > endDate.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    public static DateTime? ParseDate(int? year, int? month, int? day)
    {
        if (!year.HasValue || !month.HasValue || !day.HasValue)
        {
            return null;
        }

        DateTime? date = null;
        try
        {
            date = new DateTime(year.Value, month.Value, day.Value, CultureInfo.CurrentCulture.DateTimeFormat.Calendar);
        }
        catch { }
        return date;
    }
}
