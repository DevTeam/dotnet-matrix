// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseRepeatedOption
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Clipr)]
    public TagArguments Clipr()
    {
        var result = CliprConfiguration.ReadTags(_args);
        CliChecks.RequireTags(LibraryCatalog.Clipr, result);
        return result;
    }
}
