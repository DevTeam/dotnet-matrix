using System.Diagnostics.CodeAnalysis;
using FlatSharp;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class SerializeCollection
{
    private SimpleList _list = new();

    public void SetupFlatSharp() => _list = SerializationData.SimpleList();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatSharp)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] FlatSharp()
    {
        var buffer = new byte[SimpleList.Serializer.GetMaxSize(_list)];
        var written = SimpleList.Serializer.Write(buffer, _list);
        var payload = buffer.AsSpan(0, written).ToArray();
        SerializationChecks.SimplePayload(LibraryCatalog.FlatSharp, payload);
        return payload;
    }
}
