// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.PowerArgs,
    FeatureStatus.Unsupported,
    "A second occurrence of one named argument is rejected with DuplicateArgException, so the library accepts a sequence only as a comma-separated single value and has no repeated-option collection.")]
public partial class ParseRepeatedOption;
