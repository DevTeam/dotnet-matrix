namespace Matrix.Logging.Models;

public sealed record CapturedLogEvent(
    string Level,
    string RenderedMessage,
    string? MessageTemplate,
    Exception? Exception,
    IReadOnlyDictionary<string, object?> Properties);

