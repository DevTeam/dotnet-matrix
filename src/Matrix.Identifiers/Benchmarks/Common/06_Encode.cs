// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.Identifiers.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("Encode", 6, "Encode Numbers", "Maps a fixed number sequence to a short string.")]
[FeatureUnavailable(
    LibraryCatalog.Ulid,
    FeatureStatus.NotApplicable,
    "Ulid generates identifiers and does not encode existing numbers.")]
[FeatureUnavailable(
    LibraryCatalog.NUlid,
    FeatureStatus.NotApplicable,
    "NUlid generates identifiers and does not encode existing numbers.")]
[FeatureUnavailable(
    LibraryCatalog.UUIDNext,
    FeatureStatus.NotApplicable,
    "UUIDNext generates GUIDs and does not encode existing numbers.")]
[FeatureUnavailable(
    LibraryCatalog.MedoUuid7,
    FeatureStatus.NotApplicable,
    "Medo.Uuid7 generates UUIDs and does not encode existing numbers.")]
[FeatureUnavailable(
    LibraryCatalog.IdGen,
    FeatureStatus.NotApplicable,
    "IdGen generates snowflake identities; encoding arbitrary numbers is not its operation.")]
[FeatureUnavailable(
    LibraryCatalog.FlakeId,
    FeatureStatus.NotApplicable,
    "FlakeId generates snowflake identities; encoding arbitrary numbers is not its operation.")]
[FeatureUnavailable(
    LibraryCatalog.Ksuid,
    FeatureStatus.NotApplicable,
    "KSUID generates identifiers and does not encode existing numbers.")]
[FeatureUnavailable(
    LibraryCatalog.Nanoid,
    FeatureStatus.NotApplicable,
    "Nanoid generates random text without an encoded numeric value.")]
public partial class Encode;
