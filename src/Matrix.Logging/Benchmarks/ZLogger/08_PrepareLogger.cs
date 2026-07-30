using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLogger)]
    public bool ZLogger()
    {
        using var fixture = new ZLoggerFixture(LogLevel.Information);
        var result = fixture.Logger.IsEnabled(LogLevel.Information);
        LoggingChecks.Prepared(LibraryCatalog.ZLogger, result);
        return result;
    }
}

