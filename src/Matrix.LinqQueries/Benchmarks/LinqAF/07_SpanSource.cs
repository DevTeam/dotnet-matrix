// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.LinqAF,
    FeatureStatus.Unsupported,
    "LinqAF 3.0.0 exposes no query entry point for ReadOnlySpan<T>.")]
public partial class SpanSource;
