using System.Diagnostics.CodeAnalysis;
using FlatSharp;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class SerializeSimpleObject
{
    private SimpleTable _table = new();

    public void SetupFlatSharp() => _table = SerializationData.SimpleTable();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatSharp)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] FlatSharp()
    {
        var buffer = new byte[SimpleTable.Serializer.GetMaxSize(_table)];
        var written = SimpleTable.Serializer.Write(buffer, _table);
        var payload = buffer.AsSpan(0, written).ToArray();
        SerializationChecks.SimplePayload(LibraryCatalog.FlatSharp, payload);
        return payload;
    }
}
