// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Collection",
    5,
    "Collection",
    "Traverses three collection elements and reports an indexed failure path.")]
public partial class Collection
{
    private readonly CollectionInput _input = ValidationData.Collection();
}
