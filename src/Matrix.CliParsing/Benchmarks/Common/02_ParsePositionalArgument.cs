// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ParsePositionalArgument",
    2,
    "Parse Positional Argument",
    "Parses a required positional argument together with an option, and returns the positional value.")]
public partial class ParsePositionalArgument
{
    private readonly string[] _args = CliData.Positional;
}
