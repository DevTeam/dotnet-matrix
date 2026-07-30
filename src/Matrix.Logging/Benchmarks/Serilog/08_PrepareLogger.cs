using Serilog.Events;

namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Serilog)]
    public bool Serilog()
    {
        using var fixture = new SerilogFixture(LogEventLevel.Information);
        var result = fixture.Logger.IsEnabled(LogEventLevel.Information);
        LoggingChecks.Prepared(LibraryCatalog.Serilog, result);
        return result;
    }
}

