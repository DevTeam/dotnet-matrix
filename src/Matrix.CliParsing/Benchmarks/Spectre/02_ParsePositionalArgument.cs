using Spectre.Console.Cli;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParsePositionalArgument
{
    private readonly SpectreCompactSetup _spectre = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpectreConsoleCli)]
    public CompactArguments SpectreConsoleCli()
    {
        var result = SpectreConfiguration.ReadCompact(_spectre.Command, _spectre.Application, _args);
        CliChecks.RequireCompact(LibraryCatalog.SpectreConsoleCli, result);
        return result;
    }
}
