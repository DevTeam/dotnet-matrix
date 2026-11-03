using ZeroLog;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class StructuredProperties
{
    private ZeroLogFixture _zeroLog = null!;
    private Log _zeroLogLogger = null!;

    [GlobalSetup(Target = nameof(ZeroLog))]
    public void SetupZeroLog()
    {
        _zeroLog = new ZeroLogFixture(LogLevel.Info);
        _zeroLogLogger = _zeroLog.Logger;
    }

    [GlobalCleanup(Target = nameof(ZeroLog))]
    public void CleanupZeroLog() => _zeroLog.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZeroLog)]
    public void ZeroLog()
    {
        // ZeroLog builds an event from a LogMessage: the text is appended literally and
        // named values are attached with AppendKeyValue, so both stay queryable in the
        // event while the message renders exactly as the contract requires.
        _zeroLogLogger
            .Info()
            .Append("Order ")
            .Append(LoggingData.OrderId)
            .Append(" completed in ")
            .Append(LoggingData.ElapsedMs)
            .Append(" ms")
            .AppendKeyValue("OrderId", LoggingData.OrderId)
            .AppendKeyValue("ElapsedMs", LoggingData.ElapsedMs)
            .Log();
        LoggingChecks.Structured(LibraryCatalog.ZeroLog, _zeroLog.Sink.Last);
    }
}
