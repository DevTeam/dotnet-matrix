using System.Diagnostics.CodeAnalysis;
using Serilog.Events;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Serilog)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public bool Serilog()
    {
        using var fixture = new SerilogFixture(LogEventLevel.Information);
        var result = fixture.Logger.IsEnabled(LogEventLevel.Information);
        LoggingChecks.Prepared(LibraryCatalog.Serilog, result);
        return result;
    }
}

