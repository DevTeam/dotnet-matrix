using log4net;
using log4net.Core;

namespace Matrix.Logging.Benchmarks;

public partial class ScopeOrContext
{
    private Log4NetFixture _log4Net = null!;

    [GlobalSetup(Target = nameof(Log4Net))]
    public void SetupLog4Net() => _log4Net = new Log4NetFixture(Level.Info);

    [GlobalCleanup(Target = nameof(Log4Net))]
    public void CleanupLog4Net() => _log4Net.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Log4Net)]
    public void Log4Net()
    {
        LogicalThreadContext.Properties["RequestId"] = LoggingData.RequestId;
        try
        {
            _log4Net.Logger.Info(LoggingData.ScopeMessage);
        }
        finally
        {
            LogicalThreadContext.Properties.Remove("RequestId");
        }

        LoggingChecks.Scope(LibraryCatalog.Log4Net, _log4Net.Sink.Last);
    }
}

