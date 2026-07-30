namespace Matrix.CsvProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "StreamingRead",
    8,
    "Streaming Read",
    "Aggregates 10,000 typed rows with forward-only reading and no row materialization.")]
public partial class StreamingRead
{
    private readonly string _csv = CsvData.LargeCsv;
}

