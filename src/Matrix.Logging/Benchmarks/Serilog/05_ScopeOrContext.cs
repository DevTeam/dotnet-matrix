using Serilog.Context;
using Serilog.Events;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class ScopeOrContext
{
    private SerilogFixture _serilog = null!;
    private Serilog.Core.Logger _serilogLogger = null!;

    [GlobalSetup(Target = nameof(Serilog))]
    public void SetupSerilog()
    {
        _serilog = new SerilogFixture(LogEventLevel.Information);
        _serilogLogger = _serilog.Logger;
    }

    [GlobalCleanup(Target = nameof(Serilog))]
    public void CleanupSerilog() => _serilog.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Serilog)]
    public void Serilog()
    {
        using (LogContext.PushProperty("RequestId", LoggingData.RequestId))
        {
            _serilogLogger.Information(LoggingData.ScopeMessage);
        }

        LoggingChecks.Scope(LibraryCatalog.Serilog, _serilog.Sink.Last);
    }
}
