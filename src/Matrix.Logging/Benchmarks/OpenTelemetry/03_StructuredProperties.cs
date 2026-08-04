using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class StructuredProperties
{
    private OpenTelemetryFixture _openTelemetry = null!;
    private ILogger _openTelemetryLogger = null!;

    [GlobalSetup(Target = nameof(OpenTelemetry))]
    public void SetupOpenTelemetry()
    {
        _openTelemetry = new OpenTelemetryFixture(LogLevel.Information);
        _openTelemetryLogger = _openTelemetry.Logger;
    }

    [GlobalCleanup(Target = nameof(OpenTelemetry))]
    public void CleanupOpenTelemetry() => _openTelemetry.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.OpenTelemetry)]
    [SuppressMessage("Performance", "CA1873:Avoid potentially expensive logging")]
    public void OpenTelemetry()
    {
        _openTelemetryLogger.LogInformation(
            LoggingData.StructuredTemplate,
            LoggingData.OrderId,
            LoggingData.ElapsedMs);
        LoggingChecks.Structured(LibraryCatalog.OpenTelemetry, _openTelemetry.Sink.Last);
    }
}
