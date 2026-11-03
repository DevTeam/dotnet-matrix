using ZeroLog;
using ZeroLog.Appenders;
using ZeroLog.Formatting;

namespace Matrix.Logging;

internal sealed class ZeroLogCaptureAppender : Appender
{
    private int _count;

    public int Count => Volatile.Read(ref _count);

    public CapturedLogEvent? Last { get; private set; }

    public override void WriteMessage(LoggedMessage message)
    {
        Interlocked.Increment(ref _count);
#if MATRIX_VALIDATION
        var properties = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var keyValue in message.KeyValues)
        {
            properties[keyValue.Key] = keyValue.TryGetValue<int>(out var number)
                ? number
                : keyValue.ToString();
        }

        Last = new CapturedLogEvent(
            NormalizeLevel(message.Level),
            message.Message.ToString(),
            null,
            message.Exception,
            properties);
#endif
    }

#if MATRIX_VALIDATION
    private static string NormalizeLevel(LogLevel level) =>
        level == LogLevel.Info ? "Information" : level.ToString();
#endif
}
