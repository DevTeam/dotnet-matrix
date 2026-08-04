using log4net.Core;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class DisabledLog
{
    private Log4NetFixture _log4Net = null!;
    private log4net.ILog _log4NetLogger = null!;

    [GlobalSetup(Target = nameof(Log4Net))]
    public void SetupLog4Net()
    {
        _log4Net = new Log4NetFixture(Level.Warn);
        _log4NetLogger = _log4Net.Logger;
    }

    [GlobalCleanup(Target = nameof(Log4Net))]
    public void CleanupLog4Net() => _log4Net.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Log4Net)]
    public void Log4Net()
    {
        _log4NetLogger.Info(LoggingData.SuppressedMessage);
        LoggingChecks.Disabled(LibraryCatalog.Log4Net, _log4Net.Sink.Count);
    }
}
