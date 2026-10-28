// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SourceGeneratedDispatch",
    7,
    "Source Generated Dispatch",
    "Dispatches one query through a compile-time generated registry.")]
public partial class SourceGeneratedDispatch
{
}
