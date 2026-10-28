using System.Diagnostics.CodeAnalysis;
using MemoryPack;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class EnumRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MemoryPack)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public EnumModel? MemoryPack()
    {
        var payload = MemoryPackSerializer.Serialize(_input);
        var result = MemoryPackSerializer.Deserialize<EnumModel>(payload);
        SerializationChecks.Enum(LibraryCatalog.MemoryPack, result);
        return result;
    }
}
