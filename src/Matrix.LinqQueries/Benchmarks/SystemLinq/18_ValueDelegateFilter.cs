// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.SystemLinq,
    FeatureStatus.Unsupported,
    "System.Linq operators accept delegate instances and expose no struct-function predicate parameter.")]
public partial class ValueDelegateFilter;
