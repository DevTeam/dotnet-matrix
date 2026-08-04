using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLogger)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public bool ZLogger()
    {
        using var fixture = new ZLoggerFixture(LogLevel.Information);
        var result = fixture.Logger.IsEnabled(LogLevel.Information);
        LoggingChecks.Prepared(LibraryCatalog.ZLogger, result);
        return result;
    }
}

