using Ork.Core.Configuration;

namespace Ork.Core.Domain.Cms;

public sealed class WidgetSettings : ISettings
{
    public List<string> ActiveWidgetSystemNames { get; set; } = [];
}
