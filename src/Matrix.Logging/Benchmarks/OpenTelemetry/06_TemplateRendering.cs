using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class TemplateRendering
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
        MicrosoftLoggingMessages.Template(
            _openTelemetry.Logger,
            LoggingData.Amount,
            LoggingData.Customer,
            null);
        LoggingChecks.Template(LibraryCatalog.OpenTelemetry, _openTelemetry.Sink.Last);
    }
}
