using ZeroLog;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class ExceptionLogging
{
    private ZeroLogFixture _zeroLog = null!;
    private Log _zeroLogLogger = null!;

    [GlobalSetup(Target = nameof(ZeroLog))]
    public void SetupZeroLog()
    {
        _zeroLog = new ZeroLogFixture(LogLevel.Info);
        _zeroLogLogger = _zeroLog.Logger;
    }

    [GlobalCleanup(Target = nameof(ZeroLog))]
    public void CleanupZeroLog() => _zeroLog.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZeroLog)]
    public void ZeroLog()
    {
        _zeroLogLogger.Error(LoggingData.ExceptionMessage, _exception);
        LoggingChecks.Exception(
            LibraryCatalog.ZeroLog,
            _zeroLog.Sink.Last,
            _exception);
    }
}
