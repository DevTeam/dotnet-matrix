using ConsoleAppFramework;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseSwitch
{
    private readonly CafCapture _cafOnCapture = new();
    private readonly CafCapture _cafOffCapture = new();
    private readonly StringWriter _cafWriter = new();
    private readonly ConsoleApp.ConsoleAppBuilder _cafOnApplication;
    private readonly ConsoleApp.ConsoleAppBuilder _cafOffApplication;

    public ParseSwitch()
    {
        _cafOnApplication = CafConfiguration.Create(_cafOnCapture);
        _cafOffApplication = CafConfiguration.Create(_cafOffCapture);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ConsoleAppFramework)]
    public SwitchArguments ConsoleAppFramework()
    {
        var on = CafConfiguration.ReadSwitch(_cafOnCapture, _cafOnApplication, _withSwitch);
        CliChecks.RequireSwitchOn(LibraryCatalog.ConsoleAppFramework, on);
        var off = CafConfiguration.ReadSwitch(_cafOffCapture, _cafOffApplication, _withoutSwitch);
        CliChecks.RequireSwitchOff(LibraryCatalog.ConsoleAppFramework, off);
        return on;
    }
}
