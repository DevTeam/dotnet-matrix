using Cocona;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    private readonly CoconaCapture _coconaCapture = new();
    private readonly StringWriter _coconaWriter = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Cocona)]
    public ReportArguments Cocona()
    {
        var result = CoconaConfiguration.ReadReport(_coconaCapture, _args);
        CliChecks.RequireReport(LibraryCatalog.Cocona, result);
        return result;
    }
}
