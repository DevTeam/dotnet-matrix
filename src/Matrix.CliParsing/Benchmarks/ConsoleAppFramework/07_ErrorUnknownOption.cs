using ConsoleAppFramework;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorUnknownOption
{
    private readonly CafCapture _cafCapture = new();
    private readonly StringWriter _cafWriter = new();
    private readonly ConsoleApp.ConsoleAppBuilder _cafApplication;

    public ErrorUnknownOption()
    {
        _cafApplication = CafConfiguration.Create(_cafCapture);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ConsoleAppFramework)]
    public ParseOutcome ConsoleAppFramework()
    {
        var result = CafConfiguration.ReadFailure(_cafWriter, _cafApplication, _args);
        CliChecks.RequireFailure(LibraryCatalog.ConsoleAppFramework, result);
        return result;
    }
}
