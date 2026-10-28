using System.Diagnostics.CodeAnalysis;
using Avro.IO;
using Avro.Reflect;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class DeserializeCollection
{
    private ReflectReader<CollectionModel> _reader = null!;

    public void SetupApacheAvro()
    {
        _payload = SerializationData.CollectionAvroBytes();
        var data = new ClassCache();
        _reader = new ReflectReader<CollectionModel>(
            SerializationData.CollectionSchema,
            SerializationData.CollectionSchema,
            data);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ApacheAvro)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public CollectionModel ApacheAvro()
    {
        using var stream = new MemoryStream(_payload, false);
        var result = _reader.Read(null!, new BinaryDecoder(stream));
        SerializationChecks.Collection(LibraryCatalog.ApacheAvro, result);
        return result;
    }
}
