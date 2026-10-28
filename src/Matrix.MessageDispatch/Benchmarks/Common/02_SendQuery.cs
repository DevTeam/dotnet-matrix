// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SendQuery",
    2,
    "Send Query",
    "Dispatches one query and returns the answer its handler computed.")]
public partial class SendQuery
{
}
