using OpenTelemetry;
using OpenTelemetry.Logs;

namespace Matrix.Logging;

internal sealed class OpenTelemetryCaptureExporter : BaseExporter<LogRecord>
{
    private int _count;

    public int Count => Volatile.Read(ref _count);

    public CapturedLogEvent? Last { get; private set; }

    public override ExportResult Export(in Batch<LogRecord> batch)
    {
        foreach (var record in batch)
        {
            Interlocked.Increment(ref _count);
#if MATRIX_VALIDATION
            var properties = new Dictionary<string, object?>(StringComparer.Ordinal);
            if (record.Attributes is not null)
            {
                foreach (var attribute in record.Attributes)
                {
                    properties[attribute.Key] = attribute.Value;
                }
            }

            record.ForEachScope(
                static (scope, values) =>
                {
                    foreach (var attribute in scope)
                    {
                        values[attribute.Key] = attribute.Value;
                    }
                },
                properties);

            Last = new CapturedLogEvent(
                record.LogLevel.ToString(),
                record.FormattedMessage ?? record.Body ?? string.Empty,
                record.Body,
                record.Exception,
                properties);
#endif
        }

        return ExportResult.Success;
    }
}
