// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.Identifiers.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SortableOrder",
    8,
    "Sortable Order",
    "Compares adjacent identifiers generated at increasing timestamps to prove ordinal order follows time.")]
[FeatureUnavailable(
    LibraryCatalog.IdGen,
    FeatureStatus.NotApplicable,
    "IdGen takes its time from a shared ITimeSource guarded against moving backwards, so it has no factory that generates at an independent chosen timestamp.")]
[FeatureUnavailable(
    LibraryCatalog.Ksuid,
    FeatureStatus.NotApplicable,
    "KSUID generates only with the current clock and exposes no factory that accepts a timestamp.")]
[FeatureUnavailable(
    LibraryCatalog.Nanoid,
    FeatureStatus.NotApplicable,
    "Nanoid identifiers are random text with no time order.")]
[FeatureUnavailable(
    LibraryCatalog.Sqids,
    FeatureStatus.NotApplicable,
    "Sqids encodes numbers and has no time order.")]
[FeatureUnavailable(
    LibraryCatalog.HashidsNet,
    FeatureStatus.NotApplicable,
    "Hashids.net encodes numbers and has no time order.")]
public partial class SortableOrder;
