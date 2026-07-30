using Microsoft.Extensions.Logging;
using ZLogger;

namespace Matrix.Logging.Benchmarks;

public partial class BufferedLogging
{
    private ZLoggerFixture _zlogger = null!;

    [GlobalSetup(Target = nameof(ZLogger))]
    public void SetupZLogger() =>
        _zlogger = new ZLoggerFixture(LogLevel.Information);

    [GlobalCleanup(Target = nameof(ZLogger))]
    public void CleanupZLogger()
    {
        _zlogger.Dispose();
        LoggingChecks.Buffered(
            LibraryCatalog.ZLogger,
            _zlogger.Sink.Count,
            _zlogger.Sink.Last);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLogger)]
    public void ZLogger() =>
        _zlogger.Logger.ZLogInformation($"Buffered event");
}

