// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ReadSimpleRows",
    1,
    "Read Simple Rows",
    "Parses three CSV records and materializes every field as text.")]
public partial class ReadSimpleRows
{
    private readonly string _csv = CsvData.SimpleCsv;
}

