using NLog;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class DisabledLog
{
    private NLogFixture _nlog = null!;
    private Logger _nlogLogger = null!;

    [GlobalSetup(Target = nameof(NLog))]
    public void SetupNLog()
    {
        _nlog = new NLogFixture(LogLevel.Warn);
        _nlogLogger = _nlog.Logger;
    }

    [GlobalCleanup(Target = nameof(NLog))]
    public void CleanupNLog() => _nlog.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NLog)]
    public void NLog()
    {
        _nlogLogger.Info(LoggingData.SuppressedMessage);
        LoggingChecks.Disabled(LibraryCatalog.NLog, _nlog.Sink.Count);
    }
}
