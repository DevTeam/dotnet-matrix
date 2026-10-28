using ConsoleAppFramework;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.ConsoleAppFramework,
    FeatureStatus.Unsupported,
    "The source generator rejects a second root command in one assembly with CAF007, and the only other route is a named command reached through its command word, so the compact form cannot be parsed from the contract's command-word-free array.")]
public partial class ErrorInvalidType;
