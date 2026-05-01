using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Ork.Core;

public static class XmlHelper
{
    public static async Task<string?> XmlEncodeAsync(string? str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        str = Regex.Replace(str, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", string.Empty, RegexOptions.Compiled);

        return await XmlEncodeAsIsAsync(str);
    }

    public static async Task<string?> XmlEncodeAsIsAsync(string? str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return null;
        }

        var settings = new XmlWriterSettings
        {
            Async = true,
            ConformanceLevel = ConformanceLevel.Auto,
        };

        await using var sw = new StringWriter();
        await using var xwr = XmlWriter.Create(sw, settings);
        await xwr.WriteStringAsync(str);
        await xwr.FlushAsync();

        return sw.ToString();
    }

    public static string XmlDecode(string str)
    {
        var sb = new StringBuilder(str);

        var rez = sb
            .Replace("&quot;", "\"")
            .Replace("&apos;", "'")
            .Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Replace("&amp;", "&").ToString();

        return rez;
    }

    public static async Task<string> SerializeDateTimeAsync(DateTime dateTime)
    {
        var xmlS = new XmlSerializer(typeof(DateTime));
        var sb = new StringBuilder();
        await using var sw = new StringWriter(sb);
        xmlS.Serialize(sw, dateTime);

        return sb.ToString();
    }
}
