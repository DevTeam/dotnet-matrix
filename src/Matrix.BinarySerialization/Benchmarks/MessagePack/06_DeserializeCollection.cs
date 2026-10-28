using System.Diagnostics.CodeAnalysis;
using MessagePack;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class DeserializeCollection
{
    public void SetupMessagePack() => _payload = MessagePackSerializer.Serialize(SerializationData.Collection());

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MessagePack)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public CollectionModel MessagePack()
    {
        var result = MessagePackSerializer.Deserialize<CollectionModel>(_payload);
        SerializationChecks.Collection(LibraryCatalog.MessagePack, result);
        return result;
    }
}
