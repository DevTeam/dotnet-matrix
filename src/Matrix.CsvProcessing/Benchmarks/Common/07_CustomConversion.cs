// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "CustomConversion",
    7,
    "Custom Conversion",
    "Converts sku-NNNN fields to the matrix-owned ProductCode value type.")]
public partial class CustomConversion
{
    private readonly string _csv = CsvData.CustomConversionCsv;
}

