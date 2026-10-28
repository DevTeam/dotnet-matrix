using Spectre.Console.Cli;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    private readonly SpectreReportSetup _spectreOn = new();
    private readonly SpectreReportSetup _spectreOff = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SpectreConsoleCli)]
    public SwitchArguments SpectreConsoleCli()
    {
        var on = SpectreConfiguration.ReadSwitch(_spectreOn.Command, _spectreOn.Application, _withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.SpectreConsoleCli, on);
        var off = SpectreConfiguration.ReadSwitch(_spectreOff.Command, _spectreOff.Application, _withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.SpectreConsoleCli, off);
        return on;
    }
}
