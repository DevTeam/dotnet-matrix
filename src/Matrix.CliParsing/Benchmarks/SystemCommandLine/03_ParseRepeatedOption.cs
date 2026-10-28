using System.CommandLine;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseRepeatedOption
{
    private readonly RootCommand _scCommand = ScCommandConfiguration.Create();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemCommandLine)]
    public TagArguments SystemCommandLine()
    {
        var result = ScCommandConfiguration.ReadTags(_scCommand, _args);
        CliChecks.RequireTags(LibraryCatalog.SystemCommandLine, result);
        return result;
    }
}
