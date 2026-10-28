using System.Diagnostics.CodeAnalysis;
using ProtoBuf;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class EnumRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ProtoBufNet)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public EnumModel ProtoBufNet()
    {
        using var stream = new MemoryStream(16);
        Serializer.Serialize(stream, _input);
        stream.Position = 0;
        var result = Serializer.Deserialize<EnumModel>(stream);
        SerializationChecks.Enum(LibraryCatalog.ProtoBufNet, result);
        return result;
    }
}
