using ZLogger;

namespace Matrix.Logging;

internal sealed class ZLoggerCaptureProcessor : IAsyncLogProcessor
{
    private int _count;

    public int Count => Volatile.Read(ref _count);

    public CapturedLogEvent? Last { get; private set; }

    public void Post(IZLoggerEntry log)
    {
        Interlocked.Increment(ref _count);
#if MATRIX_VALIDATION
        var properties = new Dictionary<string, object?>(StringComparer.Ordinal);
        for (var index = 0; index < log.ParameterCount; index++)
        {
            properties[log.GetParameterKeyAsString(index)] = log.GetParameterValue(index);
        }

        if (log.LogInfo.ScopeState is { IsEmpty: false } scope)
        {
            foreach (var property in scope.Properties)
            {
                properties[property.Key] = property.Value;
            }
        }

        Last = new CapturedLogEvent(
            log.LogInfo.LogLevel.ToString(),
            log.ToString(),
            log.GetOriginalFormat(),
            log.LogInfo.Exception,
            properties);
#endif
        log.Return();
    }

    public ValueTask DisposeAsync() => default;
}

