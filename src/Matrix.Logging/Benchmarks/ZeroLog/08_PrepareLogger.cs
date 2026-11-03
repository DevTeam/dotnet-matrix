using System.Diagnostics.CodeAnalysis;
using ZeroLog;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZeroLog)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public bool ZeroLog()
    {
        using var fixture = new ZeroLogFixture(LogLevel.Info);
        var result = fixture.Logger.IsInfoEnabled;
        LoggingChecks.Prepared(LibraryCatalog.ZeroLog, result);
        return result;
    }
}
