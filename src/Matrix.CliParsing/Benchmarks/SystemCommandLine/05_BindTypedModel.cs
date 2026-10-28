using System.CommandLine;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class BindTypedModel
{
    private readonly RootCommand _scCommand = ScCommandConfiguration.Create();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemCommandLine)]
    public CompactArguments SystemCommandLine()
    {
        var result = ScCommandConfiguration.ReadCompact(_scCommand, _args);
        CliChecks.RequireCompact(LibraryCatalog.SystemCommandLine, result);
        return result;
    }
}
