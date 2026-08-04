using Microsoft.Extensions.Logging;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class DisabledLog
{
    private OpenTelemetryFixture _openTelemetry = null!;
    private ILogger _openTelemetryLogger = null!;

    [GlobalSetup(Target = nameof(OpenTelemetry))]
    public void SetupOpenTelemetry()
    {
        _openTelemetry = new OpenTelemetryFixture(LogLevel.Warning);
        _openTelemetryLogger = _openTelemetry.Logger;
    }

    [GlobalCleanup(Target = nameof(OpenTelemetry))]
    public void CleanupOpenTelemetry() => _openTelemetry.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.OpenTelemetry)]
    public void OpenTelemetry()
    {
        _openTelemetryLogger.LogInformation(LoggingData.SuppressedMessage);
        LoggingChecks.Disabled(LibraryCatalog.OpenTelemetry, _openTelemetry.Sink.Count);
    }
}
