namespace Ork.Core.Domain.Messages;

public class AdditionalTokensAddedEvent
{
    public List<string> AddtionalTokens { get; set; } = [];

    public List<string> TokenGroups { get; set; } = [];

    public MessageTemplate MessageTemplate { get; set; } = default!;

    public void AddTokens(params string[] tokens)
    {
        AddtionalTokens.AddRange(tokens);
    }
}
