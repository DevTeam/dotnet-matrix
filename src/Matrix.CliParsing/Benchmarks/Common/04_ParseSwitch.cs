// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ParseSwitch",
    4,
    "Parse Switch",
    "Parses a boolean switch on one command line and its absence on another, returning the switch state.")]
public partial class ParseSwitch
{
    private readonly string[] _withSwitch = CliData.Switch;
    private readonly string[] _withoutSwitch = CliData.WithoutSwitch;
}
