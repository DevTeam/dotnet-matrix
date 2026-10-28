using System.Diagnostics.CodeAnalysis;
using ProtoBuf;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class DeserializeCollection
{
    public void SetupProtoBufNet() => _payload = SerializationData.CollectionBytes();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ProtoBufNet)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public CollectionModel ProtoBufNet()
    {
        using var stream = new MemoryStream(_payload, false);
        var result = Serializer.Deserialize<CollectionModel>(stream);
        SerializationChecks.Collection(LibraryCatalog.ProtoBufNet, result);
        return result;
    }
}
