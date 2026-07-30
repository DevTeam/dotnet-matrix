using NLog;

namespace Matrix.Logging.Benchmarks;

public partial class DisabledLog
{
    private NLogFixture _nlog = null!;

    [GlobalSetup(Target = nameof(NLog))]
    public void SetupNLog() => _nlog = new NLogFixture(LogLevel.Warn);

    [GlobalCleanup(Target = nameof(NLog))]
    public void CleanupNLog() => _nlog.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NLog)]
    public void NLog()
    {
        _nlog.Logger.Info(LoggingData.SuppressedMessage);
        LoggingChecks.Disabled(LibraryCatalog.NLog, _nlog.Sink.Count);
    }
}

