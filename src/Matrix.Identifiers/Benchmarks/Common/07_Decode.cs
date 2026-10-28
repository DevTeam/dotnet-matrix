// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.Identifiers.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("Decode", 7, "Decode Numbers", "Maps a string back to the number sequence that produced it.")]
[FeatureUnavailable(
    LibraryCatalog.Ulid,
    FeatureStatus.NotApplicable,
    "Ulid parses identifiers and does not decode numbers.")]
[FeatureUnavailable(
    LibraryCatalog.NUlid,
    FeatureStatus.NotApplicable,
    "NUlid parses identifiers and does not decode numbers.")]
[FeatureUnavailable(
    LibraryCatalog.UUIDNext,
    FeatureStatus.NotApplicable,
    "UUIDNext parses GUIDs and does not decode numbers.")]
[FeatureUnavailable(
    LibraryCatalog.MedoUuid7,
    FeatureStatus.NotApplicable,
    "Medo.Uuid7 parses UUIDs and does not decode numbers.")]
[FeatureUnavailable(
    LibraryCatalog.IdGen,
    FeatureStatus.NotApplicable,
    "IdGen exposes identity structure, not a number-sequence codec.")]
[FeatureUnavailable(
    LibraryCatalog.FlakeId,
    FeatureStatus.NotApplicable,
    "FlakeId exposes identity structure, not a number-sequence codec.")]
[FeatureUnavailable(
    LibraryCatalog.Ksuid,
    FeatureStatus.NotApplicable,
    "KSUID parses identifiers and does not decode numbers.")]
[FeatureUnavailable(
    LibraryCatalog.Nanoid,
    FeatureStatus.NotApplicable,
    "Nanoid text carries no numbers to decode.")]
public partial class Decode;
