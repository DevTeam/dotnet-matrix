using Spectre.Console.Cli;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorMissingRequiredValue
{
    private readonly SpectreReportSetup _spectre = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpectreConsoleCli)]
    public ParseOutcome SpectreConsoleCli()
    {
        var result = SpectreConfiguration.ReadFailure(() => _spectre.Application.Run(_args));
        CliChecks.RequireFailure(LibraryCatalog.SpectreConsoleCli, result);
        return result;
    }
}
