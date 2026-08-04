// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ReadTypedRecords",
    2,
    "Read Typed Records",
    "Parses three CSV records and materializes typed scalar values.")]
public partial class ReadTypedRecords
{
    private readonly string _csv = CsvData.SimpleCsv;
}

