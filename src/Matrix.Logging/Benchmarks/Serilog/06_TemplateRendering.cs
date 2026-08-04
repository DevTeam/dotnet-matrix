using Serilog.Events;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class TemplateRendering
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
        _serilogLogger.Information(
            "Total {Amount:0.00} for {Customer:l}",
            LoggingData.Amount,
            LoggingData.Customer);
        LoggingChecks.Template(LibraryCatalog.Serilog, _serilog.Sink.Last);
    }
}
