// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseRepeatedOption
{
    private readonly TypinMemoryConsole _typinConsole = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Typin)]
    public TagArguments Typin()
    {
        var result = TypinConfiguration.ReadTags(_typinConsole, _args);
        CliChecks.RequireTags(LibraryCatalog.Typin, result);
        return result;
    }
}
