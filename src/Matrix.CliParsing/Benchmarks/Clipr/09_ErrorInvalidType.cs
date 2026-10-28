// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorInvalidType
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Clipr)]
    public ParseOutcome Clipr()
    {
        var result = CliprConfiguration.ReadCompactFailure(_args);
        CliChecks.RequireFailure(LibraryCatalog.Clipr, result);
        return result;
    }
}
