// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorInvalidType
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandLineParser)]
    public ParseOutcome CommandLineParser()
    {
        var result = CommandLineParserConfiguration.ReadCompactFailure(_args);
        CliChecks.RequireFailure(LibraryCatalog.CommandLineParser, result);
        return result;
    }
}
