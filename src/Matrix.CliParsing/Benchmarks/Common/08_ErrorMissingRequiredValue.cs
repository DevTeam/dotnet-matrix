// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ErrorMissingRequiredValue",
    8,
    "Error Missing Required Value",
    "Parses a command line without a required option and returns the failure the library reports.")]
public partial class ErrorMissingRequiredValue
{
    private readonly string[] _args = CliData.MissingRequiredOption;
}
