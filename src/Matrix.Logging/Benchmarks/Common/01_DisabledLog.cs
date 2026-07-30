namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "DisabledLog",
    1,
    "Disabled Log",
    "Submits an Information event to a logger whose minimum level is Warning.")]
public partial class DisabledLog
{
}

