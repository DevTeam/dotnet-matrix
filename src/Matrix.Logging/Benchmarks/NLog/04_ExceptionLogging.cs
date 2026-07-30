using NLog;

namespace Matrix.Logging.Benchmarks;

public partial class ExceptionLogging
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
        _nlog.Logger.Error(_exception, LoggingData.ExceptionMessage);
        LoggingChecks.Exception(
            LibraryCatalog.NLog,
            _nlog.Sink.Last,
            _exception);
    }
}

