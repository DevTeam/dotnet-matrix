using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class StructuredProperties
{
    private OpenTelemetryFixture _openTelemetry = null!;

    [GlobalSetup(Target = nameof(OpenTelemetry))]
    public void SetupOpenTelemetry() =>
        _openTelemetry = new OpenTelemetryFixture(LogLevel.Information);

    [GlobalCleanup(Target = nameof(OpenTelemetry))]
    public void CleanupOpenTelemetry() => _openTelemetry.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.OpenTelemetry)]
    public void OpenTelemetry()
    {
        _openTelemetry.Logger.LogInformation(
            LoggingData.StructuredTemplate,
            LoggingData.OrderId,
            LoggingData.ElapsedMs);
        LoggingChecks.Structured(LibraryCatalog.OpenTelemetry, _openTelemetry.Sink.Last);
    }
}
