// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SendCommand",
    1,
    "Send Command",
    "Dispatches one command and counts how many times its handler ran.")]
public partial class SendCommand
{
}
