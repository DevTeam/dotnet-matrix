using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class DeserializeCollection
{
    public void SetupMongoBson() =>
        _payload = BsonExtensionMethods.ToBson(SerializationData.Collection());

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MongoBson)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public CollectionModel? MongoBson()
    {
        var result = BsonSerializer.Deserialize<CollectionModel>(_payload);
        SerializationChecks.Collection(LibraryCatalog.MongoBson, result);
        return result;
    }
}
