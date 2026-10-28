// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    private readonly TypinMemoryConsole _typinConsole = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Typin)]
    public SwitchArguments Typin()
    {
        var on = TypinConfiguration.ReadSwitch(_typinConsole, _withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.Typin, on);
        var off = TypinConfiguration.ReadSwitch(_typinConsole, _withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.Typin, off);
        return on;
    }
}
