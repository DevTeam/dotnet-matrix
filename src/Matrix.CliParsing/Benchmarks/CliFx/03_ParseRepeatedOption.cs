// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseRepeatedOption
{
    private readonly StringWriter _clifxWriter = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CliFx)]
    public TagArguments CliFx()
    {
        var result = CliFxConfiguration.ReadTags(_args);
        CliChecks.RequireTags(LibraryCatalog.CliFx, result);
        return result;
    }
}
