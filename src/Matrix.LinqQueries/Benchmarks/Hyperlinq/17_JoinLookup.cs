// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.Hyperlinq,
    FeatureStatus.Unsupported,
    "NetFabric.Hyperlinq 3.0.0-beta9 exposes neither Join nor GroupJoin.")]
public partial class JoinLookup;
