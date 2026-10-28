// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ErrorMissingRequiredArgument",
    10,
    "Error Missing Required Argument",
    "Parses a command line without a required positional argument and returns the failure the library reports.")]
public partial class ErrorMissingRequiredArgument
{
    private readonly string[] _args = CliData.MissingRequiredArgument;
}
