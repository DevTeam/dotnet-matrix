using Serilog.Events;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class BufferedLogging
{
    private SerilogFixture _serilog = null!;
    private Serilog.Core.Logger _serilogLogger = null!;

    [GlobalSetup(Target = nameof(Serilog))]
    public void SetupSerilog()
    {
        _serilog = new SerilogFixture(LogEventLevel.Information, true);
        _serilogLogger = _serilog.Logger;
    }

    [GlobalCleanup(Target = nameof(Serilog))]
    public void CleanupSerilog()
    {
        _serilog.Dispose();
        LoggingChecks.Buffered(
            LibraryCatalog.Serilog,
            _serilog.Sink.Count,
            _serilog.Sink.Last);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Serilog)]
    public void Serilog() =>
        _serilogLogger.Information(LoggingData.BufferedMessage);
}
