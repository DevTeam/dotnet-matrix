using System.Diagnostics.CodeAnalysis;
using MemoryPack;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class SerializeNestedObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MemoryPack)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] MemoryPack()
    {
        var payload = MemoryPackSerializer.Serialize(_input);
        SerializationChecks.SimplePayload(LibraryCatalog.MemoryPack, payload);
        return payload;
    }
}
