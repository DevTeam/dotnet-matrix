using System.CommandLine;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    private readonly RootCommand _scCommand = ScCommandConfiguration.Create();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemCommandLine)]
    public ReportArguments SystemCommandLine()
    {
        var result = ScCommandConfiguration.ReadReport(_scCommand, _args);
        CliChecks.RequireReport(LibraryCatalog.SystemCommandLine, result);
        return result;
    }
}
