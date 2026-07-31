namespace Matrix.CsvProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.TinyCsvParser,
    FeatureStatus.Unsupported,
    "TinyCsvParser is a parser and does not provide a CSV writer.")]
[MatrixFeature(
    "WriteRows",
    9,
    "Write Rows",
    "Writes three records with a header to an exact LF-terminated CSV string.")]
public partial class WriteRows
{
    private readonly CsvWriteRecord[] _records = CsvData.WriteRecords;
}
