using ConsoleAppFramework;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    private readonly CafCapture _cafCapture = new();
    private readonly StringWriter _cafWriter = new();
    private readonly ConsoleApp.ConsoleAppBuilder _cafApplication;

    public GenerateHelp()
    {
        _cafApplication = CafConfiguration.Create(_cafCapture);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ConsoleAppFramework)]
    public GeneratedHelp ConsoleAppFramework()
    {
        var result = CafConfiguration.RenderHelp(_cafWriter, _cafApplication);
        CliChecks.RequireHelp(LibraryCatalog.ConsoleAppFramework, result);
        return result;
    }
}
