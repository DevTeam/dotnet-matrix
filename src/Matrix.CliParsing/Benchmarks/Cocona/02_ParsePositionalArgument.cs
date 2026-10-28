using Cocona;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParsePositionalArgument
{
    private readonly CoconaCapture _coconaCapture = new();
    private readonly StringWriter _coconaWriter = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Cocona)]
    public CompactArguments Cocona()
    {
        var result = CoconaConfiguration.ReadCompact(_coconaCapture, _args);
        CliChecks.RequireCompact(LibraryCatalog.Cocona, result);
        return result;
    }
}
