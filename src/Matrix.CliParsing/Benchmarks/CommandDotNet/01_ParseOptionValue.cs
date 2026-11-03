using CommandDotNet;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandDotNet)]
    public ReportArguments CommandDotNet()
    {
        var result = CommandDotNetConfiguration.ReadReport(_args);
        CliChecks.RequireReport(LibraryCatalog.CommandDotNet, result);
        return result;
    }
}
