using log4net.Core;

namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Log4Net)]
    public bool Log4Net()
    {
        using var fixture = new Log4NetFixture(Level.Info);
        var result = fixture.Logger.IsInfoEnabled;
        LoggingChecks.Prepared(LibraryCatalog.Log4Net, result);
        return result;
    }
}

