// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.Identifiers.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("Parse", 4, "Parse", "Reconstructs an identifier from its canonical representation.")]
[FeatureUnavailable(
    LibraryCatalog.Nanoid,
    FeatureStatus.NotApplicable,
    "Nanoid identifiers are opaque random text with no canonical value to reconstruct.")]
[FeatureUnavailable(
    LibraryCatalog.UUIDNext,
    FeatureStatus.NotApplicable,
    "UUIDNext produces a System.Guid, and parsing GUID text is defined by the platform rather than the library, so there is no library parsing step to measure.")]
[FeatureUnavailable(
    LibraryCatalog.Sqids,
    FeatureStatus.NotApplicable,
    "Sqids decodes numbers, which the decode scenario measures instead.")]
[FeatureUnavailable(
    LibraryCatalog.HashidsNet,
    FeatureStatus.NotApplicable,
    "Hashids.net decodes numbers, which the decode scenario measures instead.")]
public partial class Parse;
