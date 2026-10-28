// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorUnknownOption
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Clipr)]
    public ParseOutcome Clipr()
    {
        var result = CliprConfiguration.ReadFailure(_args);
        CliChecks.RequireFailure(LibraryCatalog.Clipr, result);
        return result;
    }
}
