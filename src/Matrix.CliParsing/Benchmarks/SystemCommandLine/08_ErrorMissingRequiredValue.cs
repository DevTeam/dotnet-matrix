using System.CommandLine;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorMissingRequiredValue
{
    private readonly RootCommand _scCommand = ScCommandConfiguration.Create();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemCommandLine)]
    public ParseOutcome SystemCommandLine()
    {
        var result = ScCommandConfiguration.ReadFailure(_scCommand, _args);
        CliChecks.RequireFailure(LibraryCatalog.SystemCommandLine, result);
        return result;
    }
}
