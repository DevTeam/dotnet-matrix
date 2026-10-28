// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ErrorInvalidType",
    9,
    "Error Invalid Type",
    "Parses a non-numeric value for an integer option and returns the failure the library reports.")]
public partial class ErrorInvalidType
{
    private readonly string[] _args = CliData.InvalidType;
}
