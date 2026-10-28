using System.Diagnostics.CodeAnalysis;
using MessagePack;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class SerializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MessagePack)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] MessagePack()
    {
        var payload = MessagePackSerializer.Serialize(_input);
        SerializationChecks.SimplePayload(LibraryCatalog.MessagePack, payload);
        return payload;
    }
}
