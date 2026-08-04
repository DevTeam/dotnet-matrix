using Serilog.Events;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class SimpleMessage
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
        _serilogLogger.Information(LoggingData.SimpleMessage);
        LoggingChecks.Simple(LibraryCatalog.Serilog, _serilog.Sink.Last);
    }
}
