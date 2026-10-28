// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "AsyncHandler",
    6,
    "Async Handler",
    "Awaits one query handler that completes asynchronously.")]
public partial class AsyncHandler
{
}
