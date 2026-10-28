using System.Diagnostics.CodeAnalysis;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class SerializeSimpleObject
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.HyperSerializer)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] HyperSerializer()
    {
        var payload = Hyper.HyperSerializer<SimpleModel>.Serialize(_input).ToArray();
        SerializationChecks.SimplePayload(LibraryCatalog.HyperSerializer, payload);
        return payload;
    }
}
