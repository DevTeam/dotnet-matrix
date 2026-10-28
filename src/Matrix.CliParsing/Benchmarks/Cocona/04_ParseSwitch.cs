using Cocona;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    private readonly CoconaCapture _coconaCapture = new();
    private readonly StringWriter _coconaWriter = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Cocona)]
    public SwitchArguments Cocona()
    {
        var on = CoconaConfiguration.ReadSwitch(_coconaCapture, _withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.Cocona, on);
        var off = CoconaConfiguration.ReadSwitch(_coconaCapture, _withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.Cocona, off);
        return on;
    }
}
