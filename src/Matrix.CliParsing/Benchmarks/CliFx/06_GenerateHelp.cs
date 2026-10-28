using CliFx.Infrastructure;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    private readonly FakeInMemoryConsole _clifxConsole = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CliFx)]
    public GeneratedHelp CliFx()
    {
        var result = CliFxConfiguration.RenderHelp(_clifxConsole);
        CliChecks.RequireHelp(LibraryCatalog.CliFx, result);
        return result;
    }
}
