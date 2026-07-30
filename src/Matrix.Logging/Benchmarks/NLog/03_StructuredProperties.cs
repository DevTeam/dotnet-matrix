using NLog;

namespace Matrix.Logging.Benchmarks;

public partial class StructuredProperties
{
    private NLogFixture _nlog = null!;

    [GlobalSetup(Target = nameof(NLog))]
    public void SetupNLog() => _nlog = new NLogFixture(LogLevel.Info);

    [GlobalCleanup(Target = nameof(NLog))]
    public void CleanupNLog() => _nlog.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NLog)]
    public void NLog()
    {
        _nlog.Logger.Info(
            LoggingData.StructuredTemplate,
            LoggingData.OrderId,
            LoggingData.ElapsedMs);
        LoggingChecks.Structured(LibraryCatalog.NLog, _nlog.Sink.Last);
    }
}

