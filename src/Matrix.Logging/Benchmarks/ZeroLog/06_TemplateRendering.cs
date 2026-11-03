using ZeroLog;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class TemplateRendering
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
        // The interpolated-string handler receives the values and the "0.00" format, and
        // ZeroLog formats them with the invariant culture by design, so neither the values
        // nor a formatted string are produced by the matrix.
        var amount = LoggingData.Amount;
        var customer = LoggingData.Customer;
        _zeroLogLogger.Info($"Total {amount:0.00} for {customer}");
        LoggingChecks.Template(LibraryCatalog.ZeroLog, _zeroLog.Sink.Last);
    }
}
