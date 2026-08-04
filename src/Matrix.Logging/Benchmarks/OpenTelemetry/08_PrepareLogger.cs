using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.OpenTelemetry)]
    public bool OpenTelemetry()
    {
        using var fixture = new OpenTelemetryFixture(LogLevel.Information);
        var result = fixture.Logger.IsEnabled(LogLevel.Information);
        LoggingChecks.Prepared(LibraryCatalog.OpenTelemetry, result);
        return result;
    }
}
