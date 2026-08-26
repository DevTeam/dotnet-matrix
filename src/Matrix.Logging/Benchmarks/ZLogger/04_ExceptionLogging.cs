using Microsoft.Extensions.Logging;
using ZLogger;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class ExceptionLogging
{
    private ZLoggerFixture _zlogger = null!;
    private ILogger _zloggerLogger = null!;

    [GlobalSetup(Target = nameof(ZLogger))]
    public void SetupZLogger()
    {
        _zlogger = new ZLoggerFixture(LogLevel.Information);
        _zloggerLogger = _zlogger.Logger;
    }

    [GlobalCleanup(Target = nameof(ZLogger))]
    public void CleanupZLogger() => _zlogger.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLogger)]
    public void ZLogger()
    {
        _zloggerLogger.ZLogError(_exception, $"Order failed");
        LoggingChecks.Exception(LibraryCatalog.ZLogger, _zlogger.Sink.Last, _exception);
    }
}
