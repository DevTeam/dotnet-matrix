using NLog;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class BufferedLogging
{
    private NLogFixture _nlog = null!;
    private Logger _nlogLogger = null!;

    [GlobalSetup(Target = nameof(NLog))]
    public void SetupNLog()
    {
        _nlog = new NLogFixture(LogLevel.Info, true);
        _nlogLogger = _nlog.Logger;
    }

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
    public void NLog() => _nlogLogger.Info(LoggingData.BufferedMessage);
}
