using System.Diagnostics.CodeAnalysis;
using FlatSharp;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class SerializeNestedObject
{
    private OrderTable _table = new();

    public void SetupFlatSharp() => _table = SerializationData.OrderTable();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatSharp)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] FlatSharp()
    {
        var buffer = new byte[OrderTable.Serializer.GetMaxSize(_table)];
        var written = OrderTable.Serializer.Write(buffer, _table);
        var payload = buffer.AsSpan(0, written).ToArray();
        SerializationChecks.SimplePayload(LibraryCatalog.FlatSharp, payload);
        return payload;
    }
}
