using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.OpenTelemetry)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public bool OpenTelemetry()
    {
        using var fixture = new OpenTelemetryFixture(LogLevel.Information);
        var result = fixture.Logger.IsEnabled(LogLevel.Information);
        LoggingChecks.Prepared(LibraryCatalog.OpenTelemetry, result);
        return result;
    }
}
