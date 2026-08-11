// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.Hyperlinq,
    FeatureStatus.Unsupported,
    "NetFabric.Hyperlinq 3.0.0-beta9 exposes only delegate-based predicate overloads; neither package assembly contains a struct-function contract.")]
public partial class ValueDelegateFilter;
