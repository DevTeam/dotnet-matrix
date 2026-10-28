// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.Identifiers.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("GenerateBatch", 2, "Generate Batch", "Fills a preallocated array of 1,024 identifiers.")]
[FeatureUnavailable(
    LibraryCatalog.Sqids,
    FeatureStatus.NotApplicable,
    "Sqids encodes existing numbers and does not generate identifiers.")]
[FeatureUnavailable(
    LibraryCatalog.HashidsNet,
    FeatureStatus.NotApplicable,
    "Hashids.net encodes existing numbers and does not generate identifiers.")]
public partial class GenerateBatch;
