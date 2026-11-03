// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.NRecoCsv,
    FeatureStatus.Unsupported,
    "NReco.Csv exposes only string fields and has no typed field access or record binding.")]
public partial class ReadTypedRecords;
