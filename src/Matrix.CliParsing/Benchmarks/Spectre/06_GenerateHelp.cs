using Spectre.Console.Cli;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    private readonly SpectreReportSetup _spectre = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpectreConsoleCli)]
    public GeneratedHelp SpectreConsoleCli()
    {
        var result = SpectreConfiguration.RenderHelp(_spectre.Application, _spectre.Writer);
        CliChecks.RequireHelp(LibraryCatalog.SpectreConsoleCli, result);
        return result;
    }
}
