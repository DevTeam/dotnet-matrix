namespace Matrix.CsvProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "HeaderMapping",
    6,
    "Header Mapping",
    "Maps a reordered CSV header to the correct typed record members.")]
public partial class HeaderMapping
{
    private readonly string _csv = CsvData.ReorderedCsv;
}

