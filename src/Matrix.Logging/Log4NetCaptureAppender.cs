using log4net.Appender;
using log4net.Core;

namespace Matrix.Logging;

internal sealed class Log4NetCaptureAppender : AppenderSkeleton
{
    private int _count;

    public int Count => Volatile.Read(ref _count);

    public CapturedLogEvent? Last { get; private set; }

    protected override void Append(LoggingEvent loggingEvent)
    {
        Interlocked.Increment(ref _count);
#if MATRIX_VALIDATION
        var properties = new Dictionary<string, object?>(StringComparer.Ordinal);
        AddProperty(loggingEvent, properties, "OrderId");
        AddProperty(loggingEvent, properties, "ElapsedMs");
        AddProperty(loggingEvent, properties, "RequestId");
        Last = new CapturedLogEvent(
            NormalizeLevel(loggingEvent.Level),
            loggingEvent.RenderedMessage ?? string.Empty,
            null,
            loggingEvent.ExceptionObject,
            properties);
#endif
    }

#if MATRIX_VALIDATION
    private static void AddProperty(
        LoggingEvent loggingEvent,
        IDictionary<string, object?> properties,
        string name)
    {
        var value = loggingEvent.LookupProperty(name);
        if (value is not null)
        {
            properties[name] = value;
        }
    }

    private static string NormalizeLevel(Level? level) =>
        level == Level.Info
            ? "Information"
            : level == Level.Error
                ? "Error"
                : level?.Name ?? string.Empty;
#endif
}
