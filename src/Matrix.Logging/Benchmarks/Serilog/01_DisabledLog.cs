using Serilog.Events;

namespace Matrix.Logging.Benchmarks;

public partial class DisabledLog
{
    private SerilogFixture _serilog = null!;

    [GlobalSetup(Target = nameof(Serilog))]
    public void SetupSerilog() =>
        _serilog = new SerilogFixture(LogEventLevel.Warning);

    [GlobalCleanup(Target = nameof(Serilog))]
    public void CleanupSerilog() => _serilog.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Serilog)]
    public void Serilog()
    {
        _serilog.Logger.Information(LoggingData.SuppressedMessage);
        LoggingChecks.Disabled(LibraryCatalog.Serilog, _serilog.Sink.Count);
    }
}

