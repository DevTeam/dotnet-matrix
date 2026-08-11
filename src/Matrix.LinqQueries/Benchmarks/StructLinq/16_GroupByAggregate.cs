// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.StructLinq,
    FeatureStatus.Unsupported,
    "StructLinq 0.28.2 exposes no GroupBy operator.")]
public partial class GroupByAggregate;
