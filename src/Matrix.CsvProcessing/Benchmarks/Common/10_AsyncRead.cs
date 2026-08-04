// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "AsyncRead",
    10,
    "Async Read",
    "Asynchronously aggregates 10,000 typed CSV rows through the library async API.")]
public partial class AsyncRead
{
    private readonly string _csv = CsvData.LargeCsv;
}

