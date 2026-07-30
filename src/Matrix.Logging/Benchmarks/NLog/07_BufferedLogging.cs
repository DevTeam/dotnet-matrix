using NLog;

namespace Matrix.Logging.Benchmarks;

public partial class BufferedLogging
{
    private NLogFixture _nlog = null!;

    [GlobalSetup(Target = nameof(NLog))]
    public void SetupNLog() => _nlog = new NLogFixture(LogLevel.Info, true);

    [GlobalCleanup(Target = nameof(NLog))]
    public void CleanupNLog()
    {
        _nlog.Flush();
        LoggingChecks.Buffered(
            LibraryCatalog.NLog,
            _nlog.Sink.Count,
            _nlog.Sink.Last);
        _nlog.Dispose();
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NLog)]
    public void NLog() => _nlog.Logger.Info(LoggingData.BufferedMessage);
}

