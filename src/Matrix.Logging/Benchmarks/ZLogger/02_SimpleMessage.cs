using Microsoft.Extensions.Logging;
using ZLogger;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class SimpleMessage
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
        _zloggerLogger.ZLogInformation($"Order accepted");
        LoggingChecks.Simple(LibraryCatalog.ZLogger, _zlogger.Sink.Last);
    }
}
