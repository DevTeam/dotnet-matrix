// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ErrorUnknownOption",
    7,
    "Error Unknown Option",
    "Parses a command line containing an undeclared option and returns the failure the library reports.")]
public partial class ErrorUnknownOption
{
    private readonly string[] _args = CliData.UnknownOption;
}
