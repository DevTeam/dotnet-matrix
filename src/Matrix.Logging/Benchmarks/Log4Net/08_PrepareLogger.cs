using System.Diagnostics.CodeAnalysis;
using log4net.Core;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class PrepareLogger
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Log4Net)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public bool Log4Net()
    {
        using var fixture = new Log4NetFixture(Level.Info);
        var result = fixture.Logger.IsInfoEnabled;
        LoggingChecks.Prepared(LibraryCatalog.Log4Net, result);
        return result;
    }
}

