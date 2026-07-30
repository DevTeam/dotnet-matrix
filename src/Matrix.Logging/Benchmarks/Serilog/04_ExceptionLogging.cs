using Serilog.Events;

namespace Matrix.Logging.Benchmarks;

public partial class ExceptionLogging
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
        _serilog.Logger.Error(_exception, LoggingData.ExceptionMessage);
        LoggingChecks.Exception(
            LibraryCatalog.Serilog,
            _serilog.Sink.Last,
            _exception);
    }
}

