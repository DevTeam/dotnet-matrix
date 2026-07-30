using NLog;

namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NLog)]
    public bool NLog()
    {
        using var fixture = new NLogFixture(LogLevel.Info);
        var result = fixture.Logger.IsInfoEnabled;
        LoggingChecks.Prepared(LibraryCatalog.NLog, result);
        return result;
    }
}

