// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "NestedObject",
    4,
    "Nested Object",
    "Traverses a nested object and reports the complete failing property path.")]
public partial class NestedObject
{
    private readonly NestedInput _input = ValidationData.Nested();
}
