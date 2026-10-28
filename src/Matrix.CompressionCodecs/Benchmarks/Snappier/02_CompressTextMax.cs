// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.CompressionCodecs.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.Snappier,
    FeatureStatus.NotApplicable,
    "Snappy defines one compression level, so it has no maximum-effort mode to compare against the levels the other codecs expose.")]
public partial class CompressTextMax;
