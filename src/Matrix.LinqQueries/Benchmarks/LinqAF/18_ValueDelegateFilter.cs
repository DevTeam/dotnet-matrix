// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.LinqAF,
    FeatureStatus.Unsupported,
    "LinqAF returns struct enumerables but its filtering operators accept ordinary delegates, not struct-function predicates.")]
public partial class ValueDelegateFilter;
