namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "PrepareLogger",
    8,
    "Prepare Logger",
    "Creates, verifies, and releases one Information-enabled logger with an in-memory sink.")]
public partial class PrepareLogger
{
}

