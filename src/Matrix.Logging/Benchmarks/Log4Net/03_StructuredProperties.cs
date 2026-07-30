using System.Globalization;
using log4net;
using log4net.Core;

namespace Matrix.Logging.Benchmarks;

public partial class StructuredProperties
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
        LogicalThreadContext.Properties["OrderId"] = LoggingData.OrderId;
        LogicalThreadContext.Properties["ElapsedMs"] = LoggingData.ElapsedMs;
        try
        {
            _log4Net.Logger.InfoFormat(
                CultureInfo.InvariantCulture,
                "Order {0} completed in {1} ms",
                LoggingData.OrderId,
                LoggingData.ElapsedMs);
        }
        finally
        {
            LogicalThreadContext.Properties.Remove("OrderId");
            LogicalThreadContext.Properties.Remove("ElapsedMs");
        }

        LoggingChecks.Structured(LibraryCatalog.Log4Net, _log4Net.Sink.Last);
    }
}

