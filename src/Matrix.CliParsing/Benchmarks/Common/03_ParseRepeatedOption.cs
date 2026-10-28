// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ParseRepeatedOption",
    3,
    "Parse Repeated Option",
    "Parses three occurrences of the same option and returns their values in the order they were supplied.")]
public partial class ParseRepeatedOption
{
    private readonly string[] _args = CliData.RepeatedOption;
}
