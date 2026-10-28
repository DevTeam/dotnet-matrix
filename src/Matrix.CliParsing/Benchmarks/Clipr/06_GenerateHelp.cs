// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Clipr)]
    public GeneratedHelp Clipr()
    {
        var result = CliprConfiguration.RenderHelp();
        CliChecks.RequireHelp(LibraryCatalog.Clipr, result);
        return result;
    }
}
