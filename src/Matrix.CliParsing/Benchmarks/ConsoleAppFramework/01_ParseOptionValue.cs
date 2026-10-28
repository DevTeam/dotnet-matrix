using ConsoleAppFramework;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParseOptionValue
{
    private readonly CafCapture _cafCapture = new();
    private readonly StringWriter _cafWriter = new();
    private readonly ConsoleApp.ConsoleAppBuilder _cafApplication;

    public ParseOptionValue()
    {
        _cafApplication = CafConfiguration.Create(_cafCapture);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ConsoleAppFramework)]
    public ReportArguments ConsoleAppFramework()
    {
        var result = CafConfiguration.ReadReport(_cafCapture, _cafApplication, _args);
        CliChecks.RequireReport(LibraryCatalog.ConsoleAppFramework, result);
        return result;
    }
}
