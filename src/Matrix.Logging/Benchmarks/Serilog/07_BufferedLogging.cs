using Serilog.Events;

namespace Matrix.Logging.Benchmarks;

public partial class BufferedLogging
{
    private SerilogFixture _serilog = null!;

    [GlobalSetup(Target = nameof(Serilog))]
    public void SetupSerilog() =>
        _serilog = new SerilogFixture(LogEventLevel.Information, true);

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
        _serilog.Logger.Information(LoggingData.BufferedMessage);
}

