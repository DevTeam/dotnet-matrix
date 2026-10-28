// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "BindTypedModel",
    5,
    "Bind Typed Model",
    "Binds the command line to a typed model, including conversion of an integer option.")]
public partial class BindTypedModel
{
    private readonly string[] _args = CliData.Positional;
}
