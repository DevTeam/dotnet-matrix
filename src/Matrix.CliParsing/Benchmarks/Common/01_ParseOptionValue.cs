// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ParseOptionValue",
    1,
    "Parse Option Value",
    "Parses an option with a value and one positional argument, and returns both values.")]
public partial class ParseOptionValue
{
    private readonly string[] _args = CliData.OptionValue;
}
