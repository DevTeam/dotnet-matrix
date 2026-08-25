// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "StructuredProperties",
    3,
    "Structured Properties",
    "Delivers one event with independently queryable OrderId and ElapsedMs properties.")]
public partial class StructuredProperties;
