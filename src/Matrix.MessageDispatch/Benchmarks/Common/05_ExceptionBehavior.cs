// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ExceptionBehavior",
    5,
    "Exception Behavior",
    "Dispatches a query whose handler throws and recovers through the exception extension point.")]
public partial class ExceptionBehavior
{
}
