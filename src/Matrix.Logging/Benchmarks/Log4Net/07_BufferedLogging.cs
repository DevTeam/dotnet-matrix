using log4net.Core;

namespace Matrix.Logging.Benchmarks;

public partial class BufferedLogging
{
    private Log4NetFixture _log4Net = null!;

    [GlobalSetup(Target = nameof(Log4Net))]
    public void SetupLog4Net() => _log4Net = new Log4NetFixture(Level.Info, true);

    [GlobalCleanup(Target = nameof(Log4Net))]
    public void CleanupLog4Net()
    {
        _log4Net.Flush();
        LoggingChecks.Buffered(
            LibraryCatalog.Log4Net,
            _log4Net.Sink.Count,
            _log4Net.Sink.Last);
        _log4Net.Dispose();
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Log4Net)]
    public void Log4Net() => _log4Net.Logger.Info(LoggingData.BufferedMessage);
}

