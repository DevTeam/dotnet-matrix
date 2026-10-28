// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandLineParser)]
    public GeneratedHelp CommandLineParser()
    {
        var result = CommandLineParserConfiguration.RenderHelp();
        CliChecks.RequireHelp(LibraryCatalog.CommandLineParser, result);
        return result;
    }
}
