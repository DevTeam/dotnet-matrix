// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    private readonly StringWriter _clifxWriter = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CliFx)]
    public ReportArguments CliFx()
    {
        var result = CliFxConfiguration.ReadReport(_args);
        CliChecks.RequireReport(LibraryCatalog.CliFx, result);
        return result;
    }
}
