using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class BufferedLogging
{
    private OpenTelemetryFixture _openTelemetry = null!;

    [GlobalSetup(Target = nameof(OpenTelemetry))]
    public void SetupOpenTelemetry() =>
        _openTelemetry = new OpenTelemetryFixture(LogLevel.Information, true);

    [GlobalCleanup(Target = nameof(OpenTelemetry))]
    public void CleanupOpenTelemetry()
    {
        var flushed = _openTelemetry.Flush();
        LoggingChecks.Flush(LibraryCatalog.OpenTelemetry, flushed);
        LoggingChecks.Buffered(
            LibraryCatalog.OpenTelemetry,
            _openTelemetry.Sink.Count,
            _openTelemetry.Sink.Last);
        _openTelemetry.Dispose();
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.OpenTelemetry)]
    public void OpenTelemetry() =>
        _openTelemetry.Logger.LogInformation(LoggingData.BufferedMessage);
}
