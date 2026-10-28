using Cocona;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class GenerateHelp
{
    private readonly CoconaCapture _coconaCapture = new();
    private readonly StringWriter _coconaWriter = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Cocona)]
    public GeneratedHelp Cocona()
    {
        var result = CoconaConfiguration.RenderHelp(_coconaWriter);
        CliChecks.RequireHelp(LibraryCatalog.Cocona, result);
        return result;
    }
}
