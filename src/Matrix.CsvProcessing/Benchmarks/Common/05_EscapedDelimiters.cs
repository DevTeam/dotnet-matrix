namespace Matrix.CsvProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "EscapedDelimiters",
    5,
    "Escaped Delimiters",
    "Parses quoted fields containing a comma or an LF newline.")]
public partial class EscapedDelimiters
{
    private readonly string _csv = CsvData.EscapedDelimitersCsv;
}

