using ConsoleAppFramework;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.ConsoleAppFramework,
    FeatureStatus.Unsupported,
    "A repeated option overwrites the previous value instead of accumulating: the array-valued parameter receives only the last --tag, so a token sequence of three tags binds one value.")]
public partial class ParseRepeatedOption;
