using CommandLine;
// ReSharper disable CheckNamespace
namespace Matrix.CliParsing.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.CommandLineParser,
    FeatureStatus.Unsupported,
    "A repeated occurrence of a sequence option is rejected with RepeatedOptionError: 2.9.1 accepts the values of a sequence only in the separator form --tag a,b,c.")]
public partial class ParseRepeatedOption;
