using System.Globalization;
using NLog;

namespace Matrix.Logging.Benchmarks;

public partial class TemplateRendering
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
            CultureInfo.InvariantCulture,
            "Total {Amount:0.00} for {Customer}",
            LoggingData.Amount,
            LoggingData.Customer);
        LoggingChecks.Template(LibraryCatalog.NLog, _nlog.Sink.Last);
    }
}

