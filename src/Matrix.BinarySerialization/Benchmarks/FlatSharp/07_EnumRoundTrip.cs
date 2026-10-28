using System.Diagnostics.CodeAnalysis;
using FlatSharp;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class EnumRoundTrip
{
    private EnumTable _table = new();

    public void SetupFlatSharp() => _table = SerializationData.EnumTable();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatSharp)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public EnumTable? FlatSharp()
    {
        var buffer = new byte[EnumTable.Serializer.GetMaxSize(_table)];
        var written = EnumTable.Serializer.Write(buffer, _table);
        var result = EnumTable.Serializer.Parse(buffer.AsSpan(0, written).ToArray());
        SerializationChecks.EnumTable(LibraryCatalog.FlatSharp, result);
        return result;
    }
}
