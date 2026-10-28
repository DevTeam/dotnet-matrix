// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "PipelineBehavior",
    4,
    "Pipeline Behavior",
    "Dispatches one query through an application-supplied pipeline behavior.")]
public partial class PipelineBehavior
{
}
