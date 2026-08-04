using System.Globalization;
using NLog;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class TemplateRendering
{
    private NLogFixture _nlog = null!;
    private Logger _nlogLogger = null!;

    [GlobalSetup(Target = nameof(NLog))]
    public void SetupNLog()
    {
        _nlog = new NLogFixture(LogLevel.Info);
        _nlogLogger = _nlog.Logger;
    }

    [GlobalCleanup(Target = nameof(NLog))]
    public void CleanupNLog() => _nlog.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NLog)]
    public void NLog()
    {
        _nlogLogger.Info(
            CultureInfo.InvariantCulture,
            "Total {Amount:0.00} for {Customer}",
            LoggingData.Amount,
            LoggingData.Customer);
        LoggingChecks.Template(LibraryCatalog.NLog, _nlog.Sink.Last);
    }
}
