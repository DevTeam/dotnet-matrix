using ZeroLog;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class BufferedLogging
{
    private ZeroLogFixture _zeroLog = null!;
    private Log _zeroLogLogger = null!;

    [GlobalSetup(Target = nameof(ZeroLog))]
    public void SetupZeroLog()
    {
        _zeroLog = new ZeroLogFixture(LogLevel.Info, true);
        _zeroLogLogger = _zeroLog.Logger;
    }

    [GlobalCleanup(Target = nameof(ZeroLog))]
    public void CleanupZeroLog()
    {
        ZeroLogFixture.Flush();
        LoggingChecks.Buffered(
            LibraryCatalog.ZeroLog,
            _zeroLog.Sink.Count,
            _zeroLog.Sink.Last);
        _zeroLog.Dispose();
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZeroLog)]
    public void ZeroLog() => _zeroLogLogger.Info(LoggingData.BufferedMessage);
}
