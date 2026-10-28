using System.Diagnostics.CodeAnalysis;

using MessagePack;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeNestedObject

{



    public void SetupMessagePack() => _payload = MessagePackSerializer.Serialize(SerializationData.Nested());



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.MessagePack)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public OrderModel MessagePack()

    {

        var result = MessagePackSerializer.Deserialize<OrderModel>(_payload);

        SerializationChecks.Nested(LibraryCatalog.MessagePack, result);

        return result;

    }

}

