namespace Matrix.Logging;

internal static class LoggingData
{
    public const string Category = "Matrix.Logging";
    public const string SuppressedMessage = "Suppressed message";
    public const string SimpleMessage = "Order accepted";
    public const string StructuredTemplate = "Order {OrderId} completed in {ElapsedMs} ms";
    public const string StructuredMessage = "Order 42 completed in 17 ms";
    public const int OrderId = 42;
    public const int ElapsedMs = 17;
    public const string ExceptionMessage = "Order failed";
    public const string ScopeMessage = "Inside request";
    public const string RequestId = "req-42";
    public const string RenderedTemplateMessage = "Total 12.50 for Ada";
    public const decimal Amount = 12.5m;
    public const string Customer = "Ada";
    public const string BufferedMessage = "Buffered event";
    public const int BufferedCapacity = 10_000;
    public const string OutputMessage = "Formatted event";

    // Equivalent plain-text layouts for FormattedOutput: timestamp, padded level, logger and message.
    public const string Log4NetOutputLayout = "%date %-5level %logger - %message";
    public const string NLogOutputLayout = "${longdate} ${level:uppercase=true:padding=-5} ${logger} - ${message}";
    public const string SerilogOutputTemplate =
        "{Timestamp:yyyy-MM-dd HH:mm:ss,fff} {Level:u5} {SourceContext} - {Message}";
}
