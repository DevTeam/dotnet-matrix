// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.Validot,
    FeatureStatus.Unsupported,
    "Validot reports a collection element as 'Items.#1.Quantity', not the required zero-based 'Items[1].Quantity', and offers no path format setting.")]
[MatrixFeature(
    "Collection",
    5,
    "Collection",
    "Traverses three collection elements and reports an indexed failure path.")]
public partial class Collection
{
    private readonly CollectionInput _input = ValidationData.Collection();
}
