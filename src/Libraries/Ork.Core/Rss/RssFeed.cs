using System.Xml.Linq;

namespace Ork.Core.Rss;

/// <summary>
/// Represents the RSS feed
/// </summary>
/// <remarks>
/// Initialize new instance of RSS feed
/// </remarks>
/// <param name="title">Title</param>
/// <param name="description">Description</param>
/// <param name="link">Link</param>
/// <param name="lastBuildDate">Last build date</param>
public sealed class RssFeed(string title, string description, Uri link, DateTimeOffset lastBuildDate)
{
    /// <summary>
    /// Initialize new instance of RSS feed
    /// </summary>
    /// <param name="link">URL</param>
    public RssFeed(Uri link) 
        : this(string.Empty, string.Empty, link, DateTimeOffset.Now)
    {
    }

    public List<XElement> ElementExtensions { get; set; } = [];

    public List<RssItem> Items { get; set; } = [];

    public XElement Title { get; set; } = new XElement(OrkRssDefaults.Title, title);

    public XElement Description { get; set; } = new XElement(OrkRssDefaults.Description, description);

    public XElement Link { get; set; } = new XElement(OrkRssDefaults.Link, link);

    public XElement LastBuildDate { get; set; } = new XElement(OrkRssDefaults.LastBuildDate, lastBuildDate.ToString("r"));

    /// <summary>
    /// Load RSS feed from the passed stream
    /// </summary>
    /// <param name="stream">Stream</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the asynchronous task whose result contains the RSS feed
    /// </returns>
    public static async Task<RssFeed?> LoadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        try
        {
            XDocument document = await XDocument.LoadAsync(stream, LoadOptions.None, cancellationToken);
            XElement? channel = document.Root?.Element(OrkRssDefaults.Channel);

            if (channel is null)
            {
                return null;
            }

            var title = channel.Element(OrkRssDefaults.Title)?.Value ?? string.Empty;
            var description = channel.Element(OrkRssDefaults.Description)?.Value ?? string.Empty;
            var link = new Uri(channel.Element(OrkRssDefaults.Link)?.Value ?? string.Empty);
            var lastBuildDateValue = channel.Element(OrkRssDefaults.LastBuildDate)?.Value;
            var lastBuildDate = lastBuildDateValue is null ? 
                DateTimeOffset.Now : 
                DateTimeOffset.ParseExact(lastBuildDateValue, "r", null);

            var feed = new RssFeed(title, description, link, lastBuildDate);

            foreach (XElement item in channel.Elements(OrkRssDefaults.Item))
            {
                feed.Items.Add(new RssItem(item));
            }

            return feed;
        }
        catch 
        {
            return null;
        }
    }
}
