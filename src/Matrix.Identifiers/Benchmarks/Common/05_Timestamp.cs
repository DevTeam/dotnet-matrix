// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.Identifiers.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("Timestamp", 5, "Extract Timestamp", "Reads the creation time embedded in a pre-generated identifier.")]
[FeatureUnavailable(
    LibraryCatalog.FlakeId,
    FeatureStatus.NotApplicable,
    "FlakeId exposes no public accessor for the timestamp inside Id; the bit layout is internal, so reading it would measure a reimplementation rather than the library.")]
[FeatureUnavailable(
    LibraryCatalog.Nanoid,
    FeatureStatus.NotApplicable,
    "Nanoid identifiers carry no timestamp.")]
[FeatureUnavailable(
    LibraryCatalog.Sqids,
    FeatureStatus.NotApplicable,
    "Sqids has no notion of time; it encodes numbers only.")]
[FeatureUnavailable(
    LibraryCatalog.HashidsNet,
    FeatureStatus.NotApplicable,
    "Hashids.net has no notion of time; it encodes numbers only.")]
public partial class Timestamp;
