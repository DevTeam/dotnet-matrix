using System.Diagnostics.CodeAnalysis;
using MemoryPack;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class DeserializeCollection
{
    public void SetupMemoryPack() => _payload = MemoryPackSerializer.Serialize(SerializationData.Collection());

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MemoryPack)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public CollectionModel? MemoryPack()
    {
        var result = MemoryPackSerializer.Deserialize<CollectionModel>(_payload);
        SerializationChecks.Collection(LibraryCatalog.MemoryPack, result);
        return result;
    }
}
