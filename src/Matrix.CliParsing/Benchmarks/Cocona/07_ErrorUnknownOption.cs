using Cocona;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ErrorUnknownOption
{
    private readonly CoconaCapture _coconaCapture = new();
    private readonly StringWriter _coconaWriter = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Cocona)]
    public ParseOutcome Cocona()
    {
        var result = CoconaConfiguration.ReadFailure(_coconaWriter, () => CoconaConfiguration.RunReport(_coconaCapture, _args));
        CliChecks.RequireFailure(LibraryCatalog.Cocona, result);
        return result;
    }
}
