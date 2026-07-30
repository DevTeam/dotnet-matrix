using Serilog.Context;
using Serilog.Events;

namespace Matrix.Logging.Benchmarks;

public partial class ScopeOrContext
{
    private SerilogFixture _serilog = null!;

    [GlobalSetup(Target = nameof(Serilog))]
    public void SetupSerilog() =>
        _serilog = new SerilogFixture(LogEventLevel.Information);

    [GlobalCleanup(Target = nameof(Serilog))]
    public void CleanupSerilog() => _serilog.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Serilog)]
    public void Serilog()
    {
        using (LogContext.PushProperty("RequestId", LoggingData.RequestId))
        {
            _serilog.Logger.Information(LoggingData.ScopeMessage);
        }

        LoggingChecks.Scope(LibraryCatalog.Serilog, _serilog.Sink.Last);
    }
}

