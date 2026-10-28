using System.Diagnostics.CodeAnalysis;
using MessagePack;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class EnumRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MessagePack)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public EnumModel MessagePack()
    {
        var payload = MessagePackSerializer.Serialize(_input);
        var result = MessagePackSerializer.Deserialize<EnumModel>(payload);
        SerializationChecks.Enum(LibraryCatalog.MessagePack, result);
        return result;
    }
}
