// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SimpleMessage",
    2,
    "Simple Message",
    "Delivers one literal Information message to an in-memory sink.")]
public partial class SimpleMessage;
