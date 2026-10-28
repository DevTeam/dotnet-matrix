// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParsePositionalArgument
{
    private readonly TypinMemoryConsole _typinConsole = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Typin)]
    public CompactArguments Typin()
    {
        var result = TypinConfiguration.ReadCompact(_typinConsole, _args);
        CliChecks.RequireCompact(LibraryCatalog.Typin, result);
        return result;
    }
}
