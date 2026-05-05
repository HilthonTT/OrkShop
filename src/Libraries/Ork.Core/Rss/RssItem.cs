using System.Xml.Linq;

namespace Ork.Core.Rss;

/// <summary>
/// Represents the item of RSS feed
/// </summary>
public sealed class RssItem
{
    /// <summary>
    /// Initialize new instance of RSS feed item
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="content">Content</param>
    /// <param name="link">Link</param>
    /// <param name="id">Unique identifier</param>
    /// <param name="pubDate">Last build date</param>
    public RssItem(string title, string content, Uri link, string id, DateTimeOffset pubDate)
    {
        Title = new XElement(OrkRssDefaults.Title, title);
        Content = new XElement(OrkRssDefaults.Description, content);
        Link = new XElement(OrkRssDefaults.Link, link);
        Id = new XElement(OrkRssDefaults.Guid, new XAttribute("isPermaLink", false), id);
        PubDate = new XElement(OrkRssDefaults.PubDate, pubDate.ToString("r"));
    }

    /// <summary>
    /// Initialize new instance of RSS feed item
    /// </summary>
    /// <param name="item">XML view of rss item</param>
    public RssItem(XContainer item)
    {
        var title = item.Element(OrkRssDefaults.Title)?.Value ?? string.Empty;
        var content = item.Element(OrkRssDefaults.Content)?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(content))
        {
            content = item.Element(OrkRssDefaults.Description)?.Value ?? string.Empty;
        }
        var link = new Uri(item.Element(OrkRssDefaults.Link)?.Value ?? string.Empty);
        var pubDateValue = item.Element(OrkRssDefaults.PubDate)?.Value;
        var pubDate = pubDateValue == null ? DateTimeOffset.Now : DateTimeOffset.ParseExact(pubDateValue, "r", null);
        var id = item.Element(OrkRssDefaults.Guid)?.Value ?? string.Empty;

        Title = new XElement(OrkRssDefaults.Title, title);
        Content = new XElement(OrkRssDefaults.Description, content);
        Link = new XElement(OrkRssDefaults.Link, link);
        Id = new XElement(OrkRssDefaults.Guid, new XAttribute("isPermaLink", false), id);
        PubDate = new XElement(OrkRssDefaults.PubDate, pubDate.ToString("r"));
    }

    public XElement Title { get; }

    public string TitleText => Title?.Value ?? string.Empty;

    public XElement Content { get; }

    public XElement Link { get; }

    public Uri Url => new(Link.Value);

    public XElement Id { get; }

    public XElement PubDate { get; }

    public DateTimeOffset PublishDate => PubDate?.Value is null 
        ? DateTimeOffset.Now : 
        DateTimeOffset.ParseExact(PubDate.Value, "r", null);

    public List<XElement> ElementExtensions { get; } = [];

    public XElement ToXElement()
    {
        var element = new XElement(OrkRssDefaults.Item, Id, Link, Title, Content);

        foreach (XElement elementExtensions in ElementExtensions)
        {
            element.Add(elementExtensions);
        }

        return element;
    }
}
