using Spectre.Console.Cli;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    private readonly SpectreReportSetup _spectre = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpectreConsoleCli)]
    public ReportArguments SpectreConsoleCli()
    {
        var result = SpectreConfiguration.ReadReport(_spectre.Command, _spectre.Application, _args);
        CliChecks.RequireReport(LibraryCatalog.SpectreConsoleCli, result);
        return result;
    }
}
