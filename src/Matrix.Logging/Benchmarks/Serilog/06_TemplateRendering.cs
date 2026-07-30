using Serilog.Events;

namespace Matrix.Logging.Benchmarks;

public partial class TemplateRendering
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
        _serilog.Logger.Information(
            "Total {Amount:0.00} for {Customer:l}",
            LoggingData.Amount,
            LoggingData.Customer);
        LoggingChecks.Template(LibraryCatalog.Serilog, _serilog.Sink.Last);
    }
}
