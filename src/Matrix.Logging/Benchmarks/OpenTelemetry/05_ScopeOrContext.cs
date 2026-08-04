using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class ScopeOrContext
{
    private OpenTelemetryFixture _openTelemetry = null!;
    private readonly Dictionary<string, object?> _openTelemetryScope =
        new(StringComparer.Ordinal)
        {
            ["RequestId"] = LoggingData.RequestId
        };

    [GlobalSetup(Target = nameof(OpenTelemetry))]
    public void SetupOpenTelemetry() =>
        _openTelemetry = new OpenTelemetryFixture(LogLevel.Information);

    [GlobalCleanup(Target = nameof(OpenTelemetry))]
    public void CleanupOpenTelemetry() => _openTelemetry.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.OpenTelemetry)]
    public void OpenTelemetry()
    {
        using (_openTelemetry.Logger.BeginScope(_openTelemetryScope))
        {
            _openTelemetry.Logger.LogInformation(LoggingData.ScopeMessage);
        }

        LoggingChecks.Scope(LibraryCatalog.OpenTelemetry, _openTelemetry.Sink.Last);
    }
}
