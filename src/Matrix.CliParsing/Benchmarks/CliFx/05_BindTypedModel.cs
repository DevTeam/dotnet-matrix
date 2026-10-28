// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class BindTypedModel
{
    private readonly StringWriter _clifxWriter = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CliFx)]
    public CompactArguments CliFx()
    {
        var result = CliFxConfiguration.ReadCompact(_args);
        CliChecks.RequireCompact(LibraryCatalog.CliFx, result);
        return result;
    }
}
