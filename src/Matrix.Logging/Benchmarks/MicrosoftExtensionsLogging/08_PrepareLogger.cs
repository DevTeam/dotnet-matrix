using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsLogging)]
    public bool MicrosoftExtensionsLogging()
    {
        using var fixture = new MicrosoftLoggingFixture(LogLevel.Information);
        var result = fixture.Logger.IsEnabled(LogLevel.Information);
        LoggingChecks.Prepared(LibraryCatalog.MicrosoftExtensionsLogging, result);
        return result;
    }
}

