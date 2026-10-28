// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

public partial class ParsePositionalArgument
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CommandLineParser)]
    public CompactArguments CommandLineParser()
    {
        var result = CommandLineParserConfiguration.ReadCompact(_args);
        CliChecks.RequireCompact(LibraryCatalog.CommandLineParser, result);
        return result;
    }
}
