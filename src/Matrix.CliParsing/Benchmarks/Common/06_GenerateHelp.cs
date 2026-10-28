// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "GenerateHelp",
    6,
    "Generate Help",
    "Builds the help text of the command, without writing it to a device.")]
public partial class GenerateHelp
{
    private readonly string[] _args = CliData.Help;
}
