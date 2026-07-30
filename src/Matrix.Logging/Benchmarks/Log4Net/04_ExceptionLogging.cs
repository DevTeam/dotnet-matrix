using log4net.Core;

namespace Matrix.Logging.Benchmarks;

public partial class ExceptionLogging
{
    private Log4NetFixture _log4Net = null!;

    [GlobalSetup(Target = nameof(Log4Net))]
    public void SetupLog4Net() => _log4Net = new Log4NetFixture(Level.Info);

    [GlobalCleanup(Target = nameof(Log4Net))]
    public void CleanupLog4Net() => _log4Net.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Log4Net)]
    public void Log4Net()
    {
        _log4Net.Logger.Error(LoggingData.ExceptionMessage, _exception);
        LoggingChecks.Exception(
            LibraryCatalog.Log4Net,
            _log4Net.Sink.Last,
            _exception);
    }
}

