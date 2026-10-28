// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Clipr)]
    public ReportArguments Clipr()
    {
        var result = CliprConfiguration.ReadReport(_args);
        CliChecks.RequireReport(LibraryCatalog.Clipr, result);
        return result;
    }
}
