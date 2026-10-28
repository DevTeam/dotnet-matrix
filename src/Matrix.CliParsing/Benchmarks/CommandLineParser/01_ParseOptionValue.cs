// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandLineParser)]
    public ReportArguments CommandLineParser()
    {
        var result = CommandLineParserConfiguration.ReadReport(_args);
        CliChecks.RequireReport(LibraryCatalog.CommandLineParser, result);
        return result;
    }
}
