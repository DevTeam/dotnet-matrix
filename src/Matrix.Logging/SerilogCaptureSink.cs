using System.Globalization;
using Serilog.Core;
using Serilog.Events;

namespace Matrix.Logging;

internal sealed class SerilogCaptureSink : ILogEventSink
{
    private int _count;

    public int Count => Volatile.Read(ref _count);

    public CapturedLogEvent? Last { get; private set; }

    public void Emit(LogEvent logEvent)
    {
        Interlocked.Increment(ref _count);
#if MATRIX_VALIDATION
        var properties = logEvent.Properties.ToDictionary(
            pair => pair.Key,
            pair => Value(pair.Value),
            StringComparer.Ordinal);
        Last = new CapturedLogEvent(
            logEvent.Level.ToString(),
            logEvent.RenderMessage(CultureInfo.InvariantCulture),
            logEvent.MessageTemplate.Text,
            logEvent.Exception,
            properties);
#endif
    }

#if MATRIX_VALIDATION
    private static object? Value(LogEventPropertyValue value) =>
        value is ScalarValue scalar ? scalar.Value : value.ToString();
#endif
}

