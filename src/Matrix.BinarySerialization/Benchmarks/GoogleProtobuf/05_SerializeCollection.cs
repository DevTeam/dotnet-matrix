using System.Diagnostics.CodeAnalysis;
using Google.Protobuf;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class SerializeCollection
{
    private CollectionMessage _message = new();

    public void SetupGoogleProtobuf() => _message = SerializationData.CollectionMessage();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.GoogleProtobuf)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] GoogleProtobuf()
    {
        var payload = _message.ToByteArray();
        SerializationChecks.SimplePayload(LibraryCatalog.GoogleProtobuf, payload);
        return payload;
    }
}
