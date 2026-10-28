using System.CommandLine;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    private readonly RootCommand _scCommand = ScCommandConfiguration.Create();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemCommandLine)]
    public GeneratedHelp SystemCommandLine()
    {
        var result = ScCommandConfiguration.RenderHelp(_scCommand);
        CliChecks.RequireHelp(LibraryCatalog.SystemCommandLine, result);
        return result;
    }
}
