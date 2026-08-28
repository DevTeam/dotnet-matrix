using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace Matrix.Logging.Aot;

internal static class AotProbe
{
    public const string Library = "OpenTelemetry";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Configures the OpenTelemetry logging provider in code and delivers one Information event
    /// to an in-memory exporter.
    /// </summary>
    /// <remarks>
    /// Microsoft.Extensions.Logging is compiled in as well, because OpenTelemetry's logging
    /// provider plugs into it and cannot be exercised without it. Any trim warning reported for
    /// this probe is therefore attributable to the pair, not to OpenTelemetry alone.
    /// </remarks>
    public static int Run()
    {
        var exporter = new CountingExporter();
        using var processor = new SimpleLogRecordExportProcessor(exporter);
        using var factory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;
                options.AddProcessor(processor);
            });
        });

        factory.CreateLogger("Matrix.Logging.Aot").LogInformation("probe");
        return exporter.Count;
    }

    private sealed class CountingExporter : BaseExporter<LogRecord>
    {
        public int Count { get; private set; }

        public override ExportResult Export(in Batch<LogRecord> batch)
        {
            Count += (int)batch.Count;
            return ExportResult.Success;
        }
    }
}
