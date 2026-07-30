using NLog;

namespace Matrix.Logging.Benchmarks;

public partial class ScopeOrContext
{
    private NLogFixture _nlog = null!;

    [GlobalSetup(Target = nameof(NLog))]
    public void SetupNLog() => _nlog = new NLogFixture(LogLevel.Info);

    [GlobalCleanup(Target = nameof(NLog))]
    public void CleanupNLog() => _nlog.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NLog)]
    public void NLog()
    {
        using (ScopeContext.PushProperty("RequestId", LoggingData.RequestId))
        {
            _nlog.Logger.Info(LoggingData.ScopeMessage);
        }

        LoggingChecks.Scope(LibraryCatalog.NLog, _nlog.Sink.Last);
    }
}

