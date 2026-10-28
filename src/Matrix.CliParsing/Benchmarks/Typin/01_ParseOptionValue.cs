// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    private readonly TypinMemoryConsole _typinConsole = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Typin)]
    public ReportArguments Typin()
    {
        var result = TypinConfiguration.ReadReport(_typinConsole, _args);
        CliChecks.RequireReport(LibraryCatalog.Typin, result);
        return result;
    }
}
