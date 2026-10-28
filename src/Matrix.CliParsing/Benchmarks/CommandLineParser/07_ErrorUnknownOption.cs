// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorUnknownOption
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandLineParser)]
    public ParseOutcome CommandLineParser()
    {
        var result = CommandLineParserConfiguration.ReadFailure(_args);
        CliChecks.RequireFailure(LibraryCatalog.CommandLineParser, result);
        return result;
    }
}
