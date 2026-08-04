using Microsoft.Extensions.Logging;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class ScopeOrContext
{
    private OpenTelemetryFixture _openTelemetry = null!;
    private ILogger _openTelemetryLogger = null!;

    private readonly Dictionary<string, object?> _openTelemetryScope =
        new(StringComparer.Ordinal)
        {
            ["RequestId"] = LoggingData.RequestId
        };

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
    public void OpenTelemetry()
    {
        using (_openTelemetryLogger.BeginScope(_openTelemetryScope))
        {
            _openTelemetryLogger.LogInformation(LoggingData.ScopeMessage);
        }

        LoggingChecks.Scope(LibraryCatalog.OpenTelemetry, _openTelemetry.Sink.Last);
    }
}
