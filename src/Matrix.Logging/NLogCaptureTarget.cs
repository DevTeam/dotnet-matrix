using NLog;
using NLog.Targets;

namespace Matrix.Logging;

internal sealed class NLogCaptureTarget : Target
{
    private int _count;

    public int Count => Volatile.Read(ref _count);

    public CapturedLogEvent? Last { get; private set; }

    protected override void Write(LogEventInfo logEvent)
    {
        Interlocked.Increment(ref _count);
#if MATRIX_VALIDATION
        var properties = logEvent.Properties.ToDictionary(
            pair => pair.Key.ToString()!,
            pair => pair.Value,
            StringComparer.Ordinal);
        if (ScopeContext.TryGetProperty("RequestId", out var requestId))
        {
            properties["RequestId"] = requestId;
        }

        Last = new CapturedLogEvent(
            NormalizeLevel(logEvent.Level),
            logEvent.FormattedMessage,
            logEvent.Message,
            logEvent.Exception,
            properties);
#endif
    }

#if MATRIX_VALIDATION
    private static string NormalizeLevel(LogLevel level) =>
        level == LogLevel.Info ? "Information" : level.Name;
#endif
}

