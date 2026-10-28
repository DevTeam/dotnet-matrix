using System.Diagnostics.CodeAnalysis;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class EnumRoundTrip
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.HyperSerializer)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public EnumModel HyperSerializer()
    {
        var payload = Hyper.HyperSerializer<EnumModel>.Serialize(_input).ToArray();
        var result = Hyper.HyperSerializer<EnumModel>.Deserialize(payload);
        SerializationChecks.Enum(LibraryCatalog.HyperSerializer, result);
        return result;
    }
}
