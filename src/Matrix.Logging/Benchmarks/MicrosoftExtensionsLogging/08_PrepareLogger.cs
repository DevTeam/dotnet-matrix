using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsLogging)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public bool MicrosoftExtensionsLogging()
    {
        using var fixture = new MicrosoftLoggingFixture(LogLevel.Information);
        var result = fixture.Logger.IsEnabled(LogLevel.Information);
        LoggingChecks.Prepared(LibraryCatalog.MicrosoftExtensionsLogging, result);
        return result;
    }
}

