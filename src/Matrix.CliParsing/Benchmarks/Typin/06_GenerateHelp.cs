// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    private readonly TypinMemoryConsole _typinConsole = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Typin)]
    public GeneratedHelp Typin()
    {
        var result = TypinConfiguration.RenderHelp(_typinConsole);
        CliChecks.RequireHelp(LibraryCatalog.Typin, result);
        return result;
    }
}
