using Spectre.Console.Cli;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseRepeatedOption
{
    private readonly SpectreReportSetup _spectre = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpectreConsoleCli)]
    public TagArguments SpectreConsoleCli()
    {
        var result = SpectreConfiguration.ReadTags(_spectre.Command, _spectre.Application, _args);
        CliChecks.RequireTags(LibraryCatalog.SpectreConsoleCli, result);
        return result;
    }
}
