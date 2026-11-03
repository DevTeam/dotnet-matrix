// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.NRecoCsv,
    FeatureStatus.Unsupported,
    "NReco.Csv has no conversion extension point: every field is returned as a string with no converter or custom-type hook.")]
public partial class CustomConversion;
