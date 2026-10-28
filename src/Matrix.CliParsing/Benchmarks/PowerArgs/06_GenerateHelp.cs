// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PowerArgs)]
    public GeneratedHelp PowerArgs()
    {
        var result = PowerArgsConfiguration.RenderHelp();
        CliChecks.RequireHelp(LibraryCatalog.PowerArgs, result);
        return result;
    }
}
