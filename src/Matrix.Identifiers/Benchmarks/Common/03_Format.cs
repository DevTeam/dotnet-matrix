// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.Identifiers.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("Format", 3, "Format", "Converts a pre-generated identifier into its canonical text.")]
[FeatureUnavailable(
    LibraryCatalog.Nanoid,
    FeatureStatus.NotApplicable,
    "Nanoid returns the identifier as text, so there is no separate formatting step.")]
[FeatureUnavailable(
    LibraryCatalog.UUIDNext,
    FeatureStatus.NotApplicable,
    "UUIDNext returns a System.Guid, and the GUID text form is defined by the platform rather than the library, so there is no library formatting step to measure.")]
[FeatureUnavailable(
    LibraryCatalog.Sqids,
    FeatureStatus.NotApplicable,
    "Sqids produces text directly and has no separate identifier value to format.")]
[FeatureUnavailable(
    LibraryCatalog.HashidsNet,
    FeatureStatus.NotApplicable,
    "Hashids.net produces text directly and has no separate identifier value to format.")]
public partial class Format;
