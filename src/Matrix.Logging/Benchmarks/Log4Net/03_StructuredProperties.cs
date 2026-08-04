using System.Globalization;
using log4net;
using log4net.Core;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class StructuredProperties
{
    private Log4NetFixture _log4Net = null!;
    private ILog _log4NetLogger = null!;

    [GlobalSetup(Target = nameof(Log4Net))]
    public void SetupLog4Net()
    {
        _log4Net = new Log4NetFixture(Level.Info);
        _log4NetLogger = _log4Net.Logger;
    }

    [GlobalCleanup(Target = nameof(Log4Net))]
    public void CleanupLog4Net() => _log4Net.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Log4Net)]
    public void Log4Net()
    {
        // ThreadContext rather than LogicalThreadContext: the other arms of this feature pass
        // structured parameters straight to the logger and use no ambient context at all, so the
        // async-flow-safe variant would charge log4net for propagation nobody else pays for. The
        // ScopeOrContext feature is where async-safe context belongs, and it still uses
        // LogicalThreadContext there, matching Serilog's LogContext.
        ThreadContext.Properties["OrderId"] = LoggingData.OrderId;
        ThreadContext.Properties["ElapsedMs"] = LoggingData.ElapsedMs;
        try
        {
            _log4NetLogger.InfoFormat(
                CultureInfo.InvariantCulture,
                "Order {0} completed in {1} ms",
                LoggingData.OrderId,
                LoggingData.ElapsedMs);
        }
        finally
        {
            ThreadContext.Properties.Remove("OrderId");
            ThreadContext.Properties.Remove("ElapsedMs");
        }

        LoggingChecks.Structured(LibraryCatalog.Log4Net, _log4Net.Sink.Last);
    }
}

