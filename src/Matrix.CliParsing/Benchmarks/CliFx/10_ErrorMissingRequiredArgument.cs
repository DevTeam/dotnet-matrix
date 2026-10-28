using CliFx.Infrastructure;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorMissingRequiredArgument
{
    private readonly FakeInMemoryConsole _clifxConsole = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CliFx)]
    public ParseOutcome CliFx()
    {
        var result = CliFxConfiguration.ReadFailure(_args, true, _clifxConsole);
        CliChecks.RequireFailure(LibraryCatalog.CliFx, result);
        return result;
    }
}
