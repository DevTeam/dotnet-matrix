// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ConditionalRule",
    6,
    "Conditional Rule",
    "Applies a tax ID rule only when the input represents a business.")]
public partial class ConditionalRule
{
    private readonly ConditionalInput _input = ValidationData.Conditional();
}
