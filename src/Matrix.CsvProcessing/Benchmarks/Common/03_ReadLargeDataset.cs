namespace Matrix.CsvProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ReadLargeDataset",
    3,
    "Read Large Dataset",
    "Parses and materializes 10,000 typed CSV records.")]
public partial class ReadLargeDataset
{
    private readonly string _csv = CsvData.LargeCsv;
}

