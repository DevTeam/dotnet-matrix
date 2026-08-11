// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.StructLinq,
    FeatureStatus.Unsupported,
    "StructLinq 0.28.2 exposes no ReadOnlySpan<T> source entry point.")]
public partial class SpanSource;
