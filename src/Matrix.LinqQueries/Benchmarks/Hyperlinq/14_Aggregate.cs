// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.Hyperlinq,
    FeatureStatus.Unsupported,
    "NetFabric.Hyperlinq 3.0.0-beta9 exposes no aggregation operator other than Count; Sum, Aggregate, Min, Max, and Average are absent.")]
public partial class Aggregate;
