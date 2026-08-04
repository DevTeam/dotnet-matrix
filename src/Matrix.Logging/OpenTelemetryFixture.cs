using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace Matrix.Logging;

internal sealed class OpenTelemetryFixture : IDisposable
{
    private readonly BaseProcessor<LogRecord> _processor;

    public OpenTelemetryFixture(LogLevel minimumLevel, bool buffered = false)
    {
        Sink = new OpenTelemetryCaptureExporter();
        _processor = buffered
            ? new BatchLogRecordExportProcessor(
                Sink,
                maxQueueSize: LoggingData.BufferedCapacity,
                scheduledDelayMilliseconds: 5_000,
                exporterTimeoutMilliseconds: 30_000,
                maxExportBatchSize: 512)
            : new SimpleLogRecordExportProcessor(Sink);
        Factory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(minimumLevel);
            builder.AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
                options.AddProcessor(_processor);
            });
        });
        Logger = Factory.CreateLogger(LoggingData.Category);
    }

    public OpenTelemetryCaptureExporter Sink { get; }

    public ILogger Logger { get; }

    private ILoggerFactory Factory { get; }

    public bool Flush() => _processor.ForceFlush(30_000);

    public void Dispose() => Factory.Dispose();
}
