// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class BindTypedModel
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Clipr)]
    public CompactArguments Clipr()
    {
        var result = CliprConfiguration.ReadCompact(_args);
        CliChecks.RequireCompact(LibraryCatalog.Clipr, result);
        return result;
    }
}
