// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.ZLinq,
    FeatureStatus.Unsupported,
    "ZLinq 1.5.6 filtering operators accept Func<T, bool> delegates and expose no struct-function predicate parameter.")]
public partial class ValueDelegateFilter;
