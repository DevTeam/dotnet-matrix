using Microsoft.Extensions.Logging;
using ZLogger;

namespace Matrix.Logging.Benchmarks;

public partial class DisabledLog
{
    private ZLoggerFixture _zlogger = null!;

    [GlobalSetup(Target = nameof(ZLogger))]
    public void SetupZLogger() =>
        _zlogger = new ZLoggerFixture(LogLevel.Warning);

    [GlobalCleanup(Target = nameof(ZLogger))]
    public void CleanupZLogger() => _zlogger.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLogger)]
    public void ZLogger()
    {
        _zlogger.Logger.ZLogInformation($"Suppressed message");
        LoggingChecks.Disabled(LibraryCatalog.ZLogger, _zlogger.Sink.Count);
    }
}

