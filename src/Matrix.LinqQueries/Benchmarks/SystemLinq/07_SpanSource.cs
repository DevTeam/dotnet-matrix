// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.SystemLinq,
    FeatureStatus.Unsupported,
    "The System.Linq.Enumerable operators accept IEnumerable<T>; no overload takes a ReadOnlySpan<T>.")]
public partial class SpanSource;
