using CommandDotNet;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseRepeatedOption
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandDotNet)]
    public TagArguments CommandDotNet()
    {
        var result = CommandDotNetConfiguration.ReadTags(_args);
        CliChecks.RequireTags(LibraryCatalog.CommandDotNet, result);
        return result;
    }
}
