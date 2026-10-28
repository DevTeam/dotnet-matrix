using Cocona;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseRepeatedOption
{
    private readonly CoconaCapture _coconaCapture = new();
    private readonly StringWriter _coconaWriter = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Cocona)]
    public TagArguments Cocona()
    {
        var result = CoconaConfiguration.ReadTags(_coconaCapture, _args);
        CliChecks.RequireTags(LibraryCatalog.Cocona, result);
        return result;
    }
}
