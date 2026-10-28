// ReSharper disable CheckNamespace
namespace Matrix.MessageDispatch.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "PublishNotification",
    3,
    "Publish Notification",
    "Publishes one notification to three handlers of the same type.")]
public partial class PublishNotification
{
}
