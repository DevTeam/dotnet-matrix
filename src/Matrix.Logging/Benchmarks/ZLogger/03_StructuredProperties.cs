using Microsoft.Extensions.Logging;
using ZLogger;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class StructuredProperties
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
        // ZLogger captures each interpolation hole under the exact identifier
        // text used here, so the local names double as the property keys
        // LoggingChecks.Structured looks up.
        var OrderId = LoggingData.OrderId;
        var ElapsedMs = LoggingData.ElapsedMs;
        _zloggerLogger.ZLogInformation($"Order {OrderId} completed in {ElapsedMs} ms");
        LoggingChecks.Structured(LibraryCatalog.ZLogger, _zlogger.Sink.Last);
    }
}
