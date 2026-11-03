using CommandDotNet;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandDotNet)]
    public GeneratedHelp CommandDotNet()
    {
        var result = CommandDotNetConfiguration.RenderHelp();
        CliChecks.RequireHelp(LibraryCatalog.CommandDotNet, result);
        return result;
    }
}
