using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class DisabledLog
{
    private OpenTelemetryFixture _openTelemetry = null!;

    [GlobalSetup(Target = nameof(OpenTelemetry))]
    public void SetupOpenTelemetry() =>
        _openTelemetry = new OpenTelemetryFixture(LogLevel.Warning);

    [GlobalCleanup(Target = nameof(OpenTelemetry))]
    public void CleanupOpenTelemetry() => _openTelemetry.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.OpenTelemetry)]
    public void OpenTelemetry()
    {
        _openTelemetry.Logger.LogInformation(LoggingData.SuppressedMessage);
        LoggingChecks.Disabled(LibraryCatalog.OpenTelemetry, _openTelemetry.Sink.Count);
    }
}
