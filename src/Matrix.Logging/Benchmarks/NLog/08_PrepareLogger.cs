using System.Diagnostics.CodeAnalysis;
using NLog;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NLog)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public bool NLog()
    {
        using var fixture = new NLogFixture(LogLevel.Info);
        var result = fixture.Logger.IsInfoEnabled;
        LoggingChecks.Prepared(LibraryCatalog.NLog, result);
        return result;
    }
}

