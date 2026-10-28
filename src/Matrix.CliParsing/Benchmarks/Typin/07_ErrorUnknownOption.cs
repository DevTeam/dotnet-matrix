// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorUnknownOption
{
    private readonly TypinMemoryConsole _typinConsole = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Typin)]
    public ParseOutcome Typin()
    {
        var result = TypinConfiguration.ReadFailure(_typinConsole, _args, false);
        CliChecks.RequireFailure(LibraryCatalog.Typin, result);
        return result;
    }
}
