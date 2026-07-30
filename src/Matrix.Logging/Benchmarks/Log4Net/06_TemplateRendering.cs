using System.Globalization;
using log4net.Core;

namespace Matrix.Logging.Benchmarks;

public partial class TemplateRendering
{
    private Log4NetFixture _log4Net = null!;

    [GlobalSetup(Target = nameof(Log4Net))]
    public void SetupLog4Net() => _log4Net = new Log4NetFixture(Level.Info);

    [GlobalCleanup(Target = nameof(Log4Net))]
    public void CleanupLog4Net() => _log4Net.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Log4Net)]
    public void Log4Net()
    {
        _log4Net.Logger.InfoFormat(
            CultureInfo.InvariantCulture,
            "Total {0:0.00} for {1}",
            LoggingData.Amount,
            LoggingData.Customer);
        LoggingChecks.Template(LibraryCatalog.Log4Net, _log4Net.Sink.Last);
    }
}

