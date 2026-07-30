namespace Matrix.CsvProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "QuotedFields",
    4,
    "Quoted Fields",
    "Parses doubled quote escapes inside quoted CSV fields.")]
public partial class QuotedFields
{
    private readonly string _csv = CsvData.QuotedCsv;
}

