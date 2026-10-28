using System.Diagnostics.CodeAnalysis;

using ProtoBuf;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeNestedObject

{



    public void SetupProtoBufNet() => _payload = SerializationData.NestedBytes();



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.ProtoBufNet)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public OrderModel ProtoBufNet()

    {

        using var stream = new MemoryStream(_payload, false);

        var result = Serializer.Deserialize<OrderModel>(stream);

        SerializationChecks.Nested(LibraryCatalog.ProtoBufNet, result);

        return result;

    }

}

